Imports HA.Common

Public Interface iLiquidItem
    Function drink(ByVal whoIsDrinking As Avatar) As String
    Function dip(ByRef whatIsBeingDipped As ItemBase) As String
End Interface

' These methods are fired when an item is equipped and de-equipped, respectively.
Public Interface iEquippableItem
	Sub activate(ByVal whoIsActivating As Avatar)
	Sub deactivate(ByVal whoIsDeactivating As Avatar)
End Interface

Public Interface iEdibleItem
    Function eat(ByVal whoIsEating As Avatar) As String
    Function bless(Optional ByVal silent As Boolean = True) As String
    Function curse(Optional ByVal silent As Boolean = True) As String
    Function decay(whoIsCarrying As Avatar) As String
    Function cook(Optional ByVal silent As Boolean = True) As String
End Interface

Public Interface iPaperItem
    Function read(ByVal whoIsReading As Avatar) As String
End Interface

Public Interface iMagicSpell
	Function cast(ByVal WhoIsCasting As Avatar) As String
	Function learn(ByVal WhoIsLearning As Avatar) As String
	Property MagicType As Enumerations.MagicType
End Interface
