using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Windows.Forms;

namespace Observatory
{
    public partial class UserInterest
    {
        public XmlNodeList AllCriteria { get; private set; }
        private ScanEvent scanEvent;
        const string criteriaFile = "ObservatoryCriteria.xml";
        private Dictionary<(string System, long Body), ScanEvent> scanHistory;
        private string currentSystem;
        private ScanEvent parent;

        public UserInterest()
        {
            bool criteriaEnabled = Properties.Observatory.Default.CustomRules;
           
            if (criteriaEnabled && File.Exists(criteriaFile))
            {
                try
                {
                    LoadCriteria();
                }
                catch (Exception ex)
                {
                    DialogResult errorResponse = MessageBox.Show("There was an error loading your custom criteria. Custom criteria will be disabled until manually re-enabled from the settings pane.\r\nDo you want to see the error details?", "Error Loading Criteria", MessageBoxButtons.YesNo);
                    if (errorResponse == DialogResult.Yes)
                    {
                        MessageBox.Show(ex.Message, "Criteria Error Detail", MessageBoxButtons.OK);
                    }
                    Properties.Observatory.Default.CustomRules = false;
                    Properties.Observatory.Default.Save();
                }
            }
            else if (criteriaEnabled)
            {
                DialogResult createFile = MessageBox.Show("Create custom criteria sample file in the same folder as the Observatory executable? You will not be asked again unless Custom Criteria are manually re-enabled.", "No Custom Criteria Found", MessageBoxButtons.YesNo);
                if (createFile == DialogResult.Yes)
                {
                    CreateTemplate(criteriaFile);
                    LoadCriteria();
                }
                else
                {
                    Properties.Observatory.Default.CustomRules = false;
                    Properties.Observatory.Default.Save();
                }
            }
        }

        private void LoadCriteria()
        {
            XmlDocument criteriaXml = new XmlDocument();
            criteriaXml.Load(criteriaFile);
            if (criteriaXml.DocumentElement.Name == "ObservatoryCriteria")
            {
                AllCriteria = criteriaXml.SelectNodes("ObservatoryCriteria/Criteria");
            }
        }

        public bool ProcessCriteria(ScanEvent scanEvent, List<(string BodyName, string Description, string Detail)> interest, Dictionary<(string System, long Body), ScanEvent> scanHistory, string currentSystem)
        {
            if (Properties.Observatory.Default.CustomRules)
            {
                this.scanHistory = scanHistory;
                this.scanEvent = scanEvent;
                this.currentSystem = currentSystem;
                bool interestingBody = false;
                foreach (XmlElement criteria in AllCriteria)
                {
                    if (CheckCriteria(criteria))
                    {
                        interest.Add((scanEvent.BodyName, criteria.SelectSingleNode("Description").InnerText, BuildDetailString(criteria.SelectSingleNode("Detail"), scanEvent)));
                        interestingBody = true;
                    }
                        
                }
                return interestingBody;
            }
            else
            {
                return false;
            }
        }

        private bool CheckCriteria(XmlNode criteria)
        {
            double compare;
            double? opResult;
            bool interesting = false;
            string opType = criteria.Attributes["Comparator"].Value.ToLower();

            switch (opType)
            {
                case "and":
                    interesting = CheckCriteria(criteria.SelectNodes("Criteria")[0]) && CheckCriteria(criteria.SelectNodes("Criteria")[1]);
                    break;
                case "or":
                    interesting = CheckCriteria(criteria.SelectNodes("Criteria")[0]) || CheckCriteria(criteria.SelectNodes("Criteria")[1]);
                    break;
                case "not":
                    interesting = !CheckCriteria(criteria.SelectSingleNode("Criteria"));
                    break;
                default:
                    compare = opType == "between" ? 0 : double.Parse(criteria.Attributes["Value"].Value);
                    opResult = EvaluateCriteriaOperation(criteria.SelectSingleNode("Operation"));
                    switch (opType)
                    {
                        case "greater":
                            interesting = (opResult > compare);
                            break;
                        case "less":
                            interesting = (opResult < compare);
                            break;
                        case "equal":
                            interesting = (opResult == compare);
                            break;
                        case "between":
                            interesting = opResult > double.Parse(criteria.Attributes["LowerValue"].Value) && opResult < double.Parse(criteria.Attributes["UpperValue"].Value);
                            break;
                    }
                    break;
            }


            return interesting;
        }

        private string BuildDetailString(XmlNode detail, ScanEvent scan)
        {
            var detailBuilder = new System.Text.StringBuilder();
            if (detail != null)
            {
                foreach (XmlElement item in detail.SelectNodes("Item"))
                {
                    detailBuilder.Append(GetDetailText(item.InnerText.ToLower(), scan));
                }
            }
            return detailBuilder.ToString();
        }

        private string GetDetailText(string detailType, ScanEvent scan)
        {
            string detailText = string.Empty;

            switch (detailType.Split(':')[0])
            {
                case "distancefromarrivalls":
                    detailText = $"Distance (LS): {scan.DistanceFromArrivalLs.ToString():N0} ";
                    break;
                case "tidallock":
                    detailText = "Tidal lock: " + (scan.TidalLock.GetValueOrDefault(false) ? "Yes " : "No ");
                    break;
                case "terraformstate":
                    detailText = scan.TerraformState?.ToLower() == "terraformable" ? "Terraformable " : "";
                    break;
                case "atmosphere":
                    detailText = scan.Atmosphere?.Length > 0 ? scan.Atmosphere + " " : "No Atmosphere ";
                    break;
                case "volcanism":
                    detailText = scan.Volcanism?.Length > 0 ? scan.Volcanism + " " : "No Volcanism ";
                    break;
                case "massem":
                    detailText = $"Mass: {scan.MassEm.GetValueOrDefault(0):N2}EM ";
                    break;
                case "radius":
                    detailText = $"Radius: {scan.Radius.GetValueOrDefault(0) / 1000:N0}km ";
                    break;
                case "surfacegravity":
                    detailText = $"Gravity: {scan.SurfaceGravity.GetValueOrDefault(0) / 9.81:N2}g ";
                    break;
                case "surfacetemperature":
                    detailText = $"Temperature: {scan.SurfaceTemperature.GetValueOrDefault(0):N0}K ";
                    break;
                case "surfacepressure":
                    detailText = $"Pressure: {scan.SurfacePressure.GetValueOrDefault(0) / 101325:N2}atm ";
                    break;
                case "landable":
                    detailText = scan.Landable.GetValueOrDefault(false) ? "Landable " : "";
                    break;
                case "semimajoraxis":
                    detailText = $"Semi-Major Axis: {scan.SemiMajorAxis.GetValueOrDefault(0) / 1000:N0}km ";
                    break;
                case "eccentricity":
                    detailText = $"Eccentricity: {scan.Eccentricity.GetValueOrDefault(0):N2} ";
                    break;
                case "orbitalinclination":
                    detailText = $"Inclination: {scan.OrbitalInclination.GetValueOrDefault(0):N2}° ";
                    break;
                case "periapsis":
                    detailText = $"Arg. of Periapsis: {scan.Periapsis.GetValueOrDefault(0):N2}° ";
                    break;
                case "orbitalperiod":
                    detailText = $"Orbital Period: {scan.OrbitalPeriod.GetValueOrDefault(0) / 86400:N1} days ";
                    break;
                case "rotationperiod":
                    detailText = $"Rotation Period: {scan.RotationPeriod.GetValueOrDefault(0) / 86400:N1} days ";
                    break;
                case "axialtilt":
                    detailText = $"Axial Tilt: {scan.AxialTilt.GetValueOrDefault(0) * 57.2958:N1}° ";
                    break;
                case "stellarmass":
                    detailText = $"Stellar Mass: {scan.StellarMass.GetValueOrDefault(0):N2}SM ";
                    break;
                case "absolutemagnitude":
                    detailText = $"Abs. Magnitude: {scan.AbsoluteMagnitude.GetValueOrDefault(0):N2} ";
                    break;
                case "age_my":
                    detailText = $"Age: {scan.Age_MY.GetValueOrDefault(0):N2}MY ";
                    break;
                case "wasdiscovered":
                    detailText = scan.WasDiscovered ? "Discovered " : "Undiscovered ";
                    break;
                case "wasmapped":
                    detailText = scan.WasMapped ? "Mapped " : "Unmapped ";
                    break;
                case "planetclass":
                    detailText = scan.PlanetClass?.Length > 0 ? scan.PlanetClass?.ToLower() : string.Empty;
                    break;
                case "rings":
                    detailText = $"{scan.Rings?.Count().ToString()} Ring{(scan.Rings?.Count() > 1 ? "s" : string.Empty)} ";
                    break;
                case "parent":

                    if (scan.Parent?.Count() > 0 && (scan.Parent?[0].ParentType == "Planet" || scan.Parent?[0].ParentType == "Star") && scanHistory.ContainsKey((currentSystem, scan.Parent[0].Body)))
                    {
                        detailText = "Parent " + GetDetailText(string.Join(":", detailType.Split(':').Skip(1).ToArray()), scanHistory[(currentSystem, scan.Parent[0].Body)]);
                    }
                    else
                    {
                        detailText = "No Parent Body";
                    }

                    break;
            }
            return detailText;
        }

        private double? EvaluateCriteriaValue(XmlNode value)
        {
            double? result = null;
            switch (value.Attributes["Type"].Value.ToLower())
            {
                case "number":
                    result = double.Parse(value.InnerText);
                    break;
                case "operation":
                    result = EvaluateCriteriaOperation(value.FirstChild);
                    break;
                case "eventdata":
                    result = GetEventValue(value.InnerText, scanEvent);
                    break;
            }
            return result;
        }

        private double? EvaluateCriteriaOperation(XmlNode operation)
        {
            double? result = null;
            string op = operation.Attributes["Operator"].Value.ToLower();
            double? firstValue = EvaluateCriteriaValue(operation.SelectSingleNode("FirstValue"));
            double? secondValue = op == "none" ? null : EvaluateCriteriaValue(operation.SelectSingleNode("SecondValue"));

            switch (op)
            {
                case "add":
                    result = firstValue + secondValue;
                    break;
                case "subtract":
                    result = firstValue - secondValue;
                    break;
                case "multiply":
                    result = firstValue * secondValue;
                    break;
                case "divide":
                    result = firstValue / secondValue;
                    break;
                case "none":
                    result = firstValue;
                    break;
            }

            return result;
        }

        private double? GetEventValue(string eventName, ScanEvent scan)
        {
            double? result = 0;

            switch (eventName.Split(':')[0].ToLower())
            {
                case "distancefromarrivalls":
                    result = scan.DistanceFromArrivalLs;
                    break;
                case "tidallock":
                    result = scan.TidalLock.HasValue ? (scan.TidalLock.GetValueOrDefault(false) ? (int?)1 : 0) : null;
                    break;
                case "terraformstate":
                    result = scan.TerraformState?.ToLower() == "terraformable" ? 1 : 0;
                    break;
                case "atmosphere":
                    result = scan.Atmosphere?.Length > 0 ? 1 : 0;
                    break;
                case "volcanism":
                    result = scan.Volcanism?.Length > 0 ? 1 : 0;
                    break;
                case "massem":
                    result = scan.MassEm;
                    break;
                case "radius":
                    result = scan.Radius;
                    break;
                case "surfacegravity":
                    result = scan.SurfaceGravity;
                    break;
                case "surfacetemperature":
                    result = scan.SurfaceTemperature;
                    break;
                case "surfacepressure":
                    result = scan.SurfacePressure;
                    break;
                case "landable":
                    result = scan.Landable.HasValue ? (scan.Landable.GetValueOrDefault(false) ? (int?)1 : 0) : null;
                    break;
                case "semimajoraxis":
                    result = scan.SemiMajorAxis;
                    break;
                case "eccentricity":
                    result = scan.Eccentricity;
                    break;
                case "orbitalinclination":
                    result = scan.OrbitalInclination;
                    break;
                case "periapsis":
                    result = scan.Periapsis;
                    break;
                case "orbitalperiod":
                    result = scan.OrbitalPeriod;
                    break;
                case "rotationperiod":
                    result = scan.RotationPeriod;
                    break;
                case "axialtilt":
                    result = scan.AxialTilt;
                    break;
                case "stellarmass":
                    result = scan.StellarMass;
                    break;
                case "absolutemagnitude":
                    result = scan.AbsoluteMagnitude;
                    break;
                case "age_my":
                    result = scan.Age_MY;
                    break;
                case "wasdiscovered":
                    result = scan.WasDiscovered ? 1 : 0;
                    break;
                case "wasmapped":
                    result = scan.WasMapped ? 1 : 0;
                    break;
                case "planetclass":
                    result = eventName.Split(':')[1].ToLower() == scan.PlanetClass?.ToLower() ? 1 : 0;
                    break;
                case "rings":
                    result = scan.Rings?.Count();
                    break;
                case "parent":
                    if (scan.Parent?.Count() > 0 && (scan.Parent?[0].ParentType == "Planet" || scan.Parent?[0].ParentType == "Star") && scanHistory.ContainsKey((currentSystem, scan.Parent[0].Body)))
                    {
                        result = GetEventValue(string.Join(":", eventName.Split(':').Skip(1).ToArray()), scanHistory[(currentSystem, scan.Parent[0].Body)]);
                        parent = scanHistory[(currentSystem, scan.Parent[0].Body)];
                    }
                    else
                    {
                        result = null;
                        parent = null;
                    }
                        
                        
                    break;
            }

            return result;
        }

        private void CreateTemplate(string path)
        {

            string template = @"<!-- Auto-generated Observatory Criteria Sample File -->
<!-- 
	Element: ObservatoryCriteria
		All criteria are contained withing a single root ObservatoryCriteria element,
		this is required and there must be only one. 
	
	Element: Criteria
		Inside the ObservatoryCriteria can be any number of Criteria elements,
		each of which must have a Comparator attribute and depending on the 
		comparator used, up to two value attributes.
		Value Comparators are ""Greater"", ""Less"", ""Equal"", and ""Between"".
		""Between"" comparisons require LowerValue and UpperValue attributes, 
		while all others use only a single Value.
		Logical Comparators without values are ""And"", ""Or"", and ""Not"".
		
		Each Criteria element must contain one Operation and one Description,
		and optionally one Detail element.
		For value comparison criteria the result of the Operation element is
		tested against the criteria's comparator and value and is added to
		Observatory's list of interesting bodies if the criteria is met.
		For logical criteria the operator specified is applied to additional
		Criteria elements nested within before any of them are added to the list.
		
		Description and Detail are ignored inside nested criteria for logical
		operations. Only the top level description and detail are used.
	
	Element: Operation
		Operation elements describe mathematical to perform	on event values,
		static numbers, or the results of other operations.
		Each Operation must have a Operator attribute, being one of ""Multiply"",
		""Divide"", ""Add"", ""Subtract"", or ""None"".
		
		The Operation element must contain up to two elements, FirstValue and
		SecondValue. The ""None"" operation uses only FirstValue, passing it's
		value unchanged, while all others require both. FirstValue and SecondValue
		are explictly ordered in this fashion to preserve the order of operands
		for subtraction and division.
	
	Element: FirstValue and SecondValue
		All value elements must have a Type attribute, which can be ""Number"",
		""EventData"", or ""Operation"".
		
		Number values contain a simple numeric value to be used by the operation.
		
		EventData values are used to get data from the scan event being processed.
		The details are found below.
			
		Operation values contain a single Operation element, the result of which
		is used as a value in the containing operation. Operations can be nested in
		this manner without limit, to create mathematical formulae of arbitrary
		complexity.
	
	Value Type: EventData
		Indicates the scan event data you want to use for a FirstValue or SecondValue
		element. All quantities are in the original units used by the journal file.
		
		Valid values are:
		
			DistanceFromArrivalLS
			TidalLock
			TerraformState
			Atmosphere
			Volcanism
			MassEM
			Radius
			SurfaceGravity
			SurfaceTemperature
			SurfacePressure
			Landable
			SemiMajorAxis
			Eccentricity
			OrbitalInclination
			Periapsis
			OrbitalPeriod
			RotationPeriod
			AxialTilt
			StellarMass
			AbsoluteMagnitude
			Age_MY
			WasDiscovered
			WasMapped
			Rings
			PlanetClass:{Class Name}
			Parent:{Parent Body Event Data}
			
		TerraformState, Atmosphere, and Volcanism return a simple 1 or 0 if they
		are populated, there is not currently a means to check for specific atmosphere
		or volcanism types. This may change in a later version.
		
		PlanetClass checks for a specific body type, as provided after the "":"". The
		class must be an exact case-insensitive match for the class text found in the
		journal file. For example ""PlanetClass:Sudarsky class V gas giant"".
		
		Rings returns a simple count of the number of rings a body has. Details of those
		rings are not yet available. This may change in a later version.
		
		Parent looks for the parent body's scan event and if found will check for the
		corresponding value, e.g., ""Parent:MassEM"". Parent data can be extended
		indefinitely if you need data for the parent's parent, or deeper if desired,
		e.g., ""Parent:Parent:Parent:PlanetClass:Earthlike Body"".
		NOTE: Parent data is only available if the parent is scanned before the child
		body. Checks that rely on parent data will fail to trigger if child bodies
		are scanned first.
		
	Element: Description
		Contains simple text which is displayed in Observatory's description column.
		
	Element: Detail
		Contains any number of Item elements to display the values of in Observatory's
		detail column, so that the numbers behind the criteria checks can be seen by
		the user, if desired.
		This element is optional.
		
	Element: Item
		Contains the scan event item you want to display the details of. Used in the
		same manner as the EventData value type, with the same valid values, with the
		exception of PlanetClass, which does not require a specific class to be
		specified.
-->

<!-- Example criteria below -->
<!-- Feel free to remove/modify as desired -->
<ObservatoryCriteria>

	<!-- Example check for greater than 1 earth-mass and tidally locked -->
	<!-- Demonstrates simple arithmetic on event values and use of detail items -->
	<Criteria Comparator=""Greater"" Value=""0"">
		<Operation Operator=""Multiply"">
			<FirstValue Type=""Operation"">
				<Operation Operator=""Subtract"">
					<FirstValue Type=""EventData"">MassEM</FirstValue>
					<SecondValue Type=""Number"">1</SecondValue>
				</Operation>
			</FirstValue>
			<SecondValue Type=""EventData"">TidalLock</SecondValue>
		</Operation>
		<Description>>1EM and Tidal Lock</Description>
		<Detail>
			<Item>MassEM</Item>
			<Item>OrbitalPeriod</Item>
		</Detail>
	</Criteria>
	
	<!-- Example check for a body larger than half the size of its parent -->
	<!-- Demonstrates use of parent event data -->
	<Criteria Comparator=""Greater"" Value=""0.5"">
		<Operation Operator=""Divide"">
			<FirstValue Type=""EventData"">Radius</FirstValue>
			<SecondValue Type=""EventData"">Parent:Radius</SecondValue>
		</Operation>
		<Description>>50% size of parent</Description>
		<Detail>
			<Item>Radius</Item>
			<Item>Parent:Radius</Item>
		</Detail>
	</Criteria>
	
	<!-- Example check for landable moon of an icy body with rings-->
	<!-- Demonstrates logical criteria and ""none"" operation -->
	<Criteria Comparator=""And"">
		<Criteria Comparator=""Greater"" Value=""0"">
			<Operation Operator=""Multiply"">
				<FirstValue Type=""EventData"">Parent:PlanetClass:Icy Body</FirstValue>
				<SecondValue Type=""EventData"">Parent:Rings</SecondValue>
			</Operation>
		</Criteria>
		<Criteria Comparator=""Equal"" Value=""1"">
			<Operation Operator=""None"">
				<FirstValue Type=""EventData"">Landable</FirstValue>
			</Operation>
		</Criteria>
		<Description>Ringed Icy Body w/ Landable Moon</Description>
		<Detail>
			<Item>Parent:Rings</Item>
		</Detail>
	</Criteria>
	
</ObservatoryCriteria>";

            StreamWriter templateFile = File.CreateText(path);
            templateFile.Write(template);
            templateFile.Close();

        }
    }
}
