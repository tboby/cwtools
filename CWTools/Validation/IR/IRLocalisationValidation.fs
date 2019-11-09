namespace CWTools.Validation.IR
open CWTools.Validation
open CWTools.Validation.LocalisationString
open CWTools.Process
open CWTools.Process.Scopes.IR
open CWTools.Common
open CWTools.Common.IRConstants
open CWTools.Utilities.Utils
open CWTools.Process.Scopes
open CWTools.Process.Scopes.Scopes
open CWTools.Common.NewScope
open CWTools.Process.Localisation
open CWTools.Process.Localisation.IR

module IRLocalisationString =

    let hardcodedLocalisation =
        [
            "playername";
            "prov"
        ]


    let commands =
        [
            "GetAreaName";
            "GetAdjective";
            "GetAdm";
            "GetCapitalName";
            "GetDip";
            "GetFlagshipName";
            "GetGroupName";
            "GetHerHim";
            "GetHerHimCap";
            "GetHerHis";
            "GetHerHisCap";
            "GetHerselfHimself";
            "GetHerselfHimselfCap";
            "GetSheHe";
            "GetSheHeCap";
            "GetMil";
            "GetName";
            "GetTitle";
            "GetTradeGoodsName";
            "GetWomanMan";
            "GetYear";
            "GovernmentName";
            "GetXEDip";
            "GetErEreDip";
            "GetDateText";
            "GetDate";
            "GetYear";
            "GetMonth";
            "GetReligiousSchool";
            "GetCultureName";
            "GetCultureGroupName";
            "GetReligionName";
            "GetReligionGroupName";
        ]
    let locCommands() = commands |> List.map (fun c -> c, scopeManager.AllScopes), []

    let validateProcessedLocalisation : ((Lang * LocKeySet) list -> (Lang * Map<string,LocEntry>) list -> ValidationResult) = validateProcessedLocalisationBase hardcodedLocalisation
    let processLocalisation = fun commands variableCommands dynamicSettings -> processLocalisationBase (localisationCommandValidator commands variableCommands dynamicSettings) defaultContext
    let validateLocalisationCommand = fun commands variableCommands dynamicSettings -> validateLocalisationCommandsBase (localisationCommandValidator commands variableCommands dynamicSettings)
