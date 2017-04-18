Imports HA.Common

Public Interface iLiquidItem
	Function drink(ByVal WhoIsDrinking As Avatar) As String
	Function dip(ByRef WhatIsBeingDipped As ItemBase) As String
End Interface

' These methods are fired when an item is equipped and de-equipped, respectively.
Public Interface iEquippableItem
	Sub activate(ByVal whoIsActivating As Avatar)
	Sub deactivate(ByVal whoIsDeactivating As Avatar)
End Interface

Public Interface iEdibleItem
	Function eat(ByVal WhoIsActivating As Avatar) As String
End Interface

Public Interface iPaperItem
	Function read(ByVal WhoIsReading As Avatar) As String
End Interface

Public Interface iMagicSpell
	Function cast(ByVal WhoIsCasting As Avatar) As String
	Function learn(ByVal WhoIsLearning As Avatar) As String
	Property MagicType As Enumerations.MagicType
End Interface
