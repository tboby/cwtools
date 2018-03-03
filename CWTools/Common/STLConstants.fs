namespace CWTools.Common

open System
module STLConstants =
    /// Blackninja9939: Country, leader, galatic object, planet, ship, fleet, pop, ambient object, army, tile, species, pop faction, sector and alliance
    /// Blackninja9939: War and Megastructure are scopes too
    type Scope =
        |Country
        |Leader
        |GalacticObject
        |Planet
        |Ship
        |Fleet
        |Pop
        |AmbientObject
        |Army
        |Tile
        |Species
        |PopFaction
        |Sector
        |Alliance
        |War
        |Megastructure
        |Any
        |Design
        |Starbase
        |Star
        |InvalidScope

    let allScopes = [
            Country;
            Leader;
            GalacticObject;
            Planet;
            Ship;
            Fleet;
            Pop;
            AmbientObject;
            Army;
            Tile
            Species;
            PopFaction;
            Sector;
            Alliance;
            War;
            Design;
            Starbase;
            Megastructure;
            Star;
            ]
    let parseScope =
        (fun (x : string) -> 
        x.ToLower() 
        |>
            function    
            |"country" -> Scope.Country
            |"leader" -> Scope.Leader
            |"galacticobject"
            |"system"
            |"galactic_object" -> Scope.GalacticObject
            |"planet" -> Scope.Planet
            |"ship" -> Scope.Ship
            |"fleet" -> Scope.Fleet
            |"pop" -> Scope.Pop
            |"ambientobject"
            |"ambient_object" -> Scope.AmbientObject
            |"army" -> Scope.Army
            |"tile" -> Scope.Tile
            |"species" -> Scope.Species
            |"popfaction"
            |"pop_faction" -> Scope.PopFaction
            |"sector" -> Scope.Sector
            |"alliance" -> Scope.Country
            |"war" -> Scope.War
            |"megastructure" -> Scope.Ship
            |"design" -> Scope.Design
            |"starbase" -> Scope.Starbase
            |"any" -> Scope.Any
            |"all" -> Scope.Any
            |x -> failwith ("unexpected scope" + x.ToString()))

    let parseScopes =
        function
        |"all" -> allScopes
        |x -> [parseScope x]

    type EffectType = |Effect |Trigger |Both
    type Effect internal (name, scopes, effectType) =
        member val Name : string = name
        member val Scopes : Scope list = scopes
        member val Type : EffectType = effectType
        override x.Equals(y) = 
            match y with
            | :? Effect as y -> x.Name = y.Name && x.Scopes = y.Scopes && x.Type = y.Type
            |_ -> false
        interface System.IComparable with
            member x.CompareTo yobj =
                match yobj with
                | :? Effect as y -> 
                    let r1 = x.Name.CompareTo(y.Name)
                    if r1 = 0 then 0 else List.compareWith compare x.Scopes y.Scopes
                | _ -> invalidArg "yobj" ("cannot compare values of different types" + yobj.GetType().ToString())
        override x.ToString() = sprintf "%s: %A" x.Name x.Scopes
    

    type ScriptedEffect(name, scopes, effectType) =
        inherit Effect(name, scopes, effectType)
        override x.Equals(y) = 
            match y with
            | :? ScriptedEffect as y -> x.Name = y.Name && x.Scopes = y.Scopes && x.Type = y.Type
            |_ -> false
        interface System.IComparable with
            member x.CompareTo yobj =
                match yobj with
                | :? Effect as y -> x.Name.CompareTo(y.Name)
                | _ -> invalidArg "yobj" "cannot compare values of different types" 

    type DocEffect(name, scopes, effectType, desc, usage) =
        inherit Effect(name, scopes, effectType)
        member val Desc : string = desc
        member val Usage : string = usage
        override x.Equals(y) =
            match y with
            | :? DocEffect as y -> x.Name = y.Name && x.Scopes = y.Scopes && x.Type = y.Type && x.Desc = y.Desc && x.Usage = y.Usage
            |_ -> false
        interface System.IComparable with
            member x.CompareTo yobj =
                match yobj with
                | :? Effect as y -> x.Name.CompareTo(y.Name)
                | _ -> invalidArg "yobj" "cannot compare values of different types" 
        new(rawEffect : RawEffect, effectType) =
            let scopes = rawEffect.scopes |> List.collect parseScopes
            DocEffect(rawEffect.name, scopes, effectType, rawEffect.desc, rawEffect.usage)
    type ScopedEffect(name, scopes, inner, effectType, desc, usage) =
        inherit DocEffect(name, scopes, effectType, desc, usage)
        member val InnerScope : Scope = inner
        new(de : DocEffect, inner : Scope) =
            ScopedEffect(de.Name, de.Scopes, inner, de.Type, de.Desc, de.Usage)

    type ModifierCategory =
        |Pop
        |Science
        |Country
        |Army
        |Leader
        |Planet
        |PopFaction
        |ShipSize
        |Ship
        |Tile
        |Megastructure
        |PlanetClass
        |Starbase
        |Any
    type RawStaticModifier = 
        {
            num : int
            tag : string
            name : string
        }
    type RawModifier = 
        {
            tag : string
            category : int
        }

    type Modifier = 
        {
            tag : string
            categories : ModifierCategory list
            /// Is this a core modifier or a static modifier?
            core : bool
        }
    let createModifier (raw : RawModifier) =
        let category = 
            match raw.category with
            | 2 -> ModifierCategory.Pop
            | 64 -> ModifierCategory.Science
            | 256 -> ModifierCategory.Country
            | 512 -> ModifierCategory.Army
            | 1024 -> ModifierCategory.Leader
            | 2048 -> ModifierCategory.Planet
            | 8192 -> ModifierCategory.PopFaction
            | 16496 -> ModifierCategory.ShipSize
            | 16508 -> ModifierCategory.Ship
            | 32768 -> ModifierCategory.Tile
            | 65536 -> ModifierCategory.Megastructure
            | 131072 -> ModifierCategory.PlanetClass
            | 262144 -> ModifierCategory.Starbase
            |_ -> ModifierCategory.Any
        { tag = raw.tag; categories = [category]; core = true}