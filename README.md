# Elite Observatory
Tool for reading/monitoring Elite Dangerous journals for interesting objects.

## How To Use
Simply run the exe anywhere on a machine with Elite Dangerous installed. It should automatically locate your journal files, but if for some reason it cannot it will ask for a folder to use.

Once running, you can scan your previously existing journals for interesting object, or begin monitoring for new events and add new discoveries on the fly.

From the settings window you have the option to disable checks for different types of discoveries, to turn on toast or text-to-speech notifications, and are given a very basic interface to edit the format of data copied out of the main list window.

## How to create custom criteria
User configurable criteria are stored in an xml file named ObservatoryCriteria.xml in the same folder as Observatory.exe. The file must have an `<ObservatoryCriteria>` root element, within which can be any number of `<Criteria>` elements.

Each `<Criteria>` contains one `<Operation>` element, one `<Description>` element, and one optional `<Detail>` element.

A `<Criteria>` element must have `Comparator` and `Value` attributes. The result of the `<Operation>` is compared against the `Value` using the `Comparator`.  Allowed `Comparator`s are "Greater", "Less", and "Equal".

An `<Operation>` element requires an `Operator` attribute, and `<FirstValue>` and `<SecondValue>` elements. The operator is applied to the first and second values, in that order in cases where it matters, such as division. Allowed `Operator`s are "Add", "Subtract", "Multiply", and "Divide".

The `<FirstValue>` and `<SecondValue>` elements must have a `Type` attribute. This can be one of "Number", "EventData", or "Operation". Number values are simply that, numbers places inside the element. Operations allow you to next another `<Operation>` element inside the previous operation. EventData is a text descriptor matching a key from the Elite Dangerous journal. Not all keys work yet, a list is provided below.

The `<Description>` element is simple text which appears in the Description column of the main window when your criteria is found.

The `<Detail>` element contains any number of `<Item>` elements, each of which contains a key from the Elite Dangerous journal, much like the EventData value type, but in this case serves as a list of values to add to the detail column of the main window.

The following is a sample criteria triggered by objects with high axial tilt (>1 radian). Note that the values used for criteria use the units of measurement of the journal file itself, not the friendlier values normally displayed to end users. In this case the tilt is given in radians. Also note that since axial tilt can be both positive or negative, there are two checks, one for above 1, another for below -1.

```xml
<ObservatoryCriteria>
	<Criteria Comparator="Greater" Value="1">
		<Operation Operator="Add">
			<FirstValue Type="EventData">AxialTilt</FirstValue>
			<SecondValue Type="Number">0</SecondValue>
		</Operation>
		<Description>High Axial Tilt</Description>
		<Detail>
			<Item>AxialTilt</Item>
		</Detail>
	</Criteria>
	<Criteria Comparator="Less" Value="-1">
		<Operation Operator="Add">
			<FirstValue Type="EventData">AxialTilt</FirstValue>
			<SecondValue Type="Number">0</SecondValue>
		</Operation>
		<Description>High Axial Tilt</Description>
		<Detail>
			<Item>AxialTilt</Item>
		</Detail>
	</Criteria>
</ObservatoryCriteria>
```

A sample criteria file including comments about its use can be created automatically by Elite Observatory if you choose. The single criteria it contains is probably impossible, unless the Stellar Forge does something really crazy, but it can give you an idea of how to construct criteria of your own.

### Valid EventData and Item values
```
DistanceFromArrivalLs
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
```

Atmosphere, Volcanism, Landable, WasDiscovered, and WasMapped return simple 1 or 0 values when used as EventData.

## Prerequisites for use
.NET 4.5, and by extension Windows Vista or later.

## Prerequisites for building
C# 7.0, Newtonsoft.Json, System.ValueTuple

Optional: ILMerge
