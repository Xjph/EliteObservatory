using System;
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
                DialogResult createFile = MessageBox.Show("Create custom criteria sample file in the same folder as the Observatory executable? You will not be asked again.", "No Custom Criteria Found", MessageBoxButtons.YesNo);
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

        public bool CheckCriteria(ScanEvent scanEvent, List<(string BodyName, string Description, string Detail)> interest)
        {
            if (Properties.Observatory.Default.CustomRules)
            {
                this.scanEvent = scanEvent;
                bool interestingBody = false;
                foreach (XmlElement criteria in AllCriteria)
                {
                    bool interesting = false;
                    double opResult = EvaluateCriteriaOperation(criteria.SelectSingleNode("Operation"));
                    double compare = double.Parse(criteria.Attributes["Value"].Value);
                    switch (criteria.Attributes["Comparator"].Value.ToLower())
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
                    }

                    if (interesting)
                    {
                        interestingBody = true;
                        interest.Add((scanEvent.BodyName, criteria.SelectSingleNode("Description").InnerText, BuildDetailString(criteria.SelectSingleNode("Detail"))));
                    }
                }
                return interestingBody;
            }
            else
            {
                return false;
            }
        }

        private string BuildDetailString(XmlNode detail)
        {
            var detailBuilder = new System.Text.StringBuilder();
            if (detail != null)
            {
                foreach (XmlElement item in detail.SelectNodes("Item"))
                {
                    switch (item.InnerText.ToLower())
                    {
                        case "distancefromarrivalls":
                            detailBuilder.Append($"Distance (LS): {scanEvent.DistanceFromArrivalLs.ToString():N0} ");
                            break;
                        case "tidallock":
                            detailBuilder.Append("Tidal lock: " + (scanEvent.TidalLock.GetValueOrDefault(false) ? "Yes " : "No "));
                            break;
                        case "terraformstate":
                            detailBuilder.Append(scanEvent.TerraformState?.ToLower() == "terraformable" ? "Terraformable " : "");
                            break;
                        case "atmosphere":
                            detailBuilder.Append(scanEvent.Atmosphere?.Length > 0 ? scanEvent.Atmosphere + " " : "No Atmosphere ");
                            break;
                        case "volcanism":
                            detailBuilder.Append(scanEvent.Volcanism?.Length > 0 ? scanEvent.Volcanism + " " : "No Volcanism ");
                            break;
                        case "massem":
                            detailBuilder.Append($"Mass: {scanEvent.MassEm.GetValueOrDefault(0):N2}EM ");
                            break;
                        case "radius":
                            detailBuilder.Append($"Radius: {scanEvent.Radius.GetValueOrDefault(0) / 1000:N0}km ");
                            break;
                        case "surfacegravity":
q                            detailBuilder.Append($"Gravity: {scanEvent.SurfaceGravity.GetValueOrDefault(0) / 9.81:N2}g ");
                            break;
                        case "surfacetemperature":
                            detailBuilder.Append($"Temperature: {scanEvent.SurfaceTemperature.GetValueOrDefault(0):N0}K ");
                            break;
                        case "surfacepressure":
                            detailBuilder.Append($"Pressure: {scanEvent.SurfacePressure.GetValueOrDefault(0) / 101325:N2}atm ");
                            break;
                        case "landable":
                            detailBuilder.Append(scanEvent.Landable.GetValueOrDefault(false) ? "Landable " : "");
                            break;
                        case "semimajoraxis":
                            detailBuilder.Append($"Semi-Major Axis: {scanEvent.SemiMajorAxis.GetValueOrDefault(0) / 1000:N0}km ");
                            break;
                        case "eccentricity":
                            detailBuilder.Append($"Eccentricity: {scanEvent.Eccentricity.GetValueOrDefault(0):N2} ");
                            break;
                        case "orbitalinclination":
                            detailBuilder.Append($"Inclination: {scanEvent.OrbitalInclination.GetValueOrDefault(0):N2}° ");
                            break;
                        case "periapsis":
                            detailBuilder.Append($"Arg. of Periapsis: {scanEvent.Periapsis.GetValueOrDefault(0):N2}° ");
                            break;
                        case "orbitalperiod":
                            detailBuilder.Append($"Orbital Period: {scanEvent.OrbitalPeriod.GetValueOrDefault(0) / 86400:N1} days ");
                            break;
                        case "rotationperiod":
                            detailBuilder.Append($"Rotation Period: {scanEvent.RotationPeriod.GetValueOrDefault(0) / 86400:N1} days ");
                            break;
                        case "axialtilt":
                            detailBuilder.Append($"Axial Tilt: {scanEvent.AxialTilt.GetValueOrDefault(0) * 57.2958:N1}° ");
                            break;
                        case "stellarmass":
                            detailBuilder.Append($"Stellar Mass: {scanEvent.StellarMass.GetValueOrDefault(0):N2}SM ");
                            break;
                        case "absolutemagnitude":
                            detailBuilder.Append($"Abs. Magnitude: {scanEvent.AbsoluteMagnitude.GetValueOrDefault(0):N2} ");
                            break;
                        case "age_my":
                            detailBuilder.Append($"Age: {scanEvent.Age_MY.GetValueOrDefault(0):N2}MY ");
                            break;
                        case "wasdiscovered":
                            detailBuilder.Append(scanEvent.WasDiscovered ? "Discovered " : "Undiscovered ");
                            break;
                        case "wasmapped":
                            detailBuilder.Append(scanEvent.WasMapped ? "Mapped " : "Unmapped ");
                            break;
                    }
                }
            }
            return detailBuilder.ToString();
        }

        private double EvaluateCriteriaValue(XmlNode value)
        {
            double result = 0;
            switch (value.Attributes["Type"].Value.ToLower())
            {
                case "number":
                    result = double.Parse(value.InnerText);
                    break;
                case "operation":
                    result = EvaluateCriteriaOperation(value.FirstChild);
                    break;
                case "eventdata":
                    result = GetEventValue(value.InnerText);
                    break;
            }
            return result;
        }

        private double EvaluateCriteriaOperation(XmlNode operation)
        {
            double result = 0;

            double firstValue = EvaluateCriteriaValue(operation.SelectSingleNode("FirstValue"));
            double secondValue = EvaluateCriteriaValue(operation.SelectSingleNode("SecondValue"));

            switch (operation.Attributes["Operator"].Value.ToLower())
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
            }

            return result;
        }

        private double GetEventValue(string eventName)
        {
            double result = 0;

            switch (eventName.Split(':')[0].ToLower())
            {
                case "distancefromarrivalls":
                    result = scanEvent.DistanceFromArrivalLs;
                    break;
                case "tidallock":
                    result = scanEvent.TidalLock.GetValueOrDefault(false) ? 1 : 0;
                    break;
                case "terraformstate":
                    result = scanEvent.TerraformState?.ToLower() == "terraformable" ? 1 : 0;
                    break;
                case "atmosphere":
                    result = scanEvent.Atmosphere?.Length > 0 ? 1 : 0;
                    break;
                case "volcanism":
                    result = scanEvent.Volcanism?.Length > 0 ? 1 : 0;
                    break;
                case "massem":
                    result = scanEvent.MassEm.GetValueOrDefault(0);
                    break;
                case "radius":
                    result = scanEvent.Radius.GetValueOrDefault(0);
                    break;
                case "surfacegravity":
                    result = scanEvent.SurfaceGravity.GetValueOrDefault(0);
                    break;
                case "surfacetemperature":
                    result = scanEvent.SurfaceTemperature.GetValueOrDefault(0);
                    break;
                case "surfacepressure":
                    result = scanEvent.SurfacePressure.GetValueOrDefault(0);
                    break;
                case "landable":
                    result = scanEvent.Landable.GetValueOrDefault(false) ? 1 : 0;
                    break;
                case "semimajoraxis":
                    result = scanEvent.SemiMajorAxis.GetValueOrDefault(0);
                    break;
                case "eccentricity":
                    result = scanEvent.Eccentricity.GetValueOrDefault(0);
                    break;
                case "orbitalinclination":
                    result = scanEvent.OrbitalInclination.GetValueOrDefault(0);
                    break;
                case "periapsis":
                    result = scanEvent.Periapsis.GetValueOrDefault(0);
                    break;
                case "orbitalperiod":
                    result = scanEvent.OrbitalPeriod.GetValueOrDefault(0);
                    break;
                case "rotationperiod":
                    result = scanEvent.RotationPeriod.GetValueOrDefault(0);
                    break;
                case "axialtilt":
                    result = scanEvent.AxialTilt.GetValueOrDefault(0);
                    break;
                case "stellarmass":
                    result = scanEvent.StellarMass.GetValueOrDefault(0);
                    break;
                case "absolutemagnitude":
                    result = scanEvent.AbsoluteMagnitude.GetValueOrDefault(0);
                    break;
                case "age_my":
                    result = scanEvent.Age_MY.GetValueOrDefault(0);
                    break;
                case "wasdiscovered":
                    result = scanEvent.WasDiscovered ? 1 : 0;
                    break;
                case "wasmapped":
                    result = scanEvent.WasMapped ? 1 : 0;
                    break;
                case "planetclass":
                    result = eventName.Split(':')[1].ToLower() == scanEvent.PlanetClass?.ToLower() ? 1 : 0;
                    break;
            }

            return result;
        }

        private void CreateTemplate(string path)
        {
            XmlDocument template = new XmlDocument();
            template.AppendChild(template.CreateComment("Auto-generated Observatory Criteria Sample File"));

            XmlElement root = template.CreateElement("ObservatoryCriteria");
            root.AppendChild(template.CreateComment("Place each custom criteria in its own <Criteria> element"));
            
            XmlElement criteria = template.CreateElement("Criteria");
            criteria.AppendChild(template.CreateComment("Each Criteria must contain Comparator and Value attributes, one Operation element, one Description element, and optionally one Detail element"));
            criteria.AppendChild(template.CreateComment("Valid comparators are \"Less\", \"Greater\", and \"Equal\". Value must be a number"));
            criteria.AppendChild(template.CreateComment("The value is compared using the comparator against the result of the Operation element"));
            criteria.Attributes.Append(template.CreateAttribute("Comparator"));
            criteria.Attributes["Comparator"].Value = "Greater";
            criteria.Attributes.Append(template.CreateAttribute("Value"));
            criteria.Attributes["Value"].Value = "0";

            XmlElement outerOperation = template.CreateElement("Operation");
            outerOperation.AppendChild(template.CreateComment("An operation must contain a Operator attribute and FirstValue and SecondValue elements"));
            outerOperation.AppendChild(template.CreateComment("Valid operators are \"Add\", \"Subtract\", \"Multiply\", and \"Divide\""));
            outerOperation.AppendChild(template.CreateComment("Both FirstValue and Second value must have a Type attribute"));
            outerOperation.AppendChild(template.CreateComment("Valid types are \"EventData\", \"Number\", and \"Operation\""));
            outerOperation.AppendChild(template.CreateComment("EventData values pull numbers from scan events, Number values are simple numeric data, and Operation values contain another Operation element."));
            outerOperation.AppendChild(template.CreateComment("Operation elements can be nested indefinitely following the same pattern of an operation inside a value inside an operation."));
            
            outerOperation.Attributes.Append(template.CreateAttribute("Operator"));
            outerOperation.Attributes["Operator"].Value = "Multiply";

            XmlElement outerFirstValue = template.CreateElement("FirstValue");
            outerFirstValue.Attributes.Append(template.CreateAttribute("Type"));
            outerFirstValue.Attributes["Type"].Value = "EventData";
            outerFirstValue.InnerText = "Landable";

            XmlElement outerSecondValue = template.CreateElement("SecondValue");
            outerSecondValue.Attributes.Append(template.CreateAttribute("Type"));
            outerSecondValue.Attributes["Type"].Value = "Operation";

            XmlElement operation = template.CreateElement("Operation");
            operation.Attributes.Append(template.CreateAttribute("Operator"));
            operation.Attributes["Operator"].Value = "Subtract";

            XmlElement firstValue = template.CreateElement("FirstValue");
            firstValue.Attributes.Append(template.CreateAttribute("Type"));
            firstValue.Attributes["Type"].Value = "EventData";
            firstValue.InnerText = "Radius";

            XmlElement secondValue = template.CreateElement("SecondValue");
            secondValue.Attributes.Append(template.CreateAttribute("Type"));
            secondValue.Attributes["Type"].Value = "EventData";
            secondValue.InnerText = "SemiMajorAxis";

            XmlElement description = template.CreateElement("Description");
            description.InnerText = "Radius greater than Semi-Major Axis and Landable";

            XmlElement detail = template.CreateElement("Detail");
            detail.AppendChild(template.CreateComment("The Detail element contains any number of Item elements, each holding the name of a piece of data from the scan event to add to the detail column."));
            XmlElement detailItemOne = template.CreateElement("Item");
            detailItemOne.InnerText = "Radius";
            XmlElement detailItemTwo = template.CreateElement("Item");
            detailItemTwo.InnerText = "SemiMajorAxis";
            detail.AppendChild(detailItemOne);
            detail.AppendChild(detailItemTwo);

            operation.AppendChild(firstValue);
            operation.AppendChild(secondValue);
            
            outerSecondValue.AppendChild(operation);
            outerOperation.AppendChild(outerFirstValue);
            outerOperation.AppendChild(outerSecondValue);
            criteria.AppendChild(outerOperation);
            criteria.AppendChild(template.CreateComment("Description is the text that appears in the description column"));
            criteria.AppendChild(description);
            criteria.AppendChild(detail);

            root.AppendChild(criteria);
            template.AppendChild(root);
            template.Save(path);
        }
    }
}
