[<AutoOpen>]
module Extensions

open FsAst
open FSharp.Compiler.SyntaxTree
open FSharp.Compiler.Range
open FSharp.Compiler.XmlDoc
open Fantomas

type SynFieldRcd with
    static member Create(name: string, fieldType: SynType) =
        {
            Access = None
            Attributes = [ ]
            Id = Some (Ident.Create name)
            IsMutable = false
            IsStatic = false
            Range = range0
            Type = fieldType
            XmlDoc= PreXmlDoc.Empty
        }

    static member Create(name: string, fieldType: string) =
        {
            Access = None
            Attributes = [ ]
            Id = Some (Ident.Create name)
            IsMutable = false
            IsStatic = false
            Range = range0
            Type = SynType.Create fieldType
            XmlDoc= PreXmlDoc.Empty
        }

type SynMemberDefn with
    /// <summary>
    /// Creates a member from a binding definition: [static member {binding} = {expr}]
    /// where {binding} = {pattern args} and {expr} is the body of the static binding
    /// </summary>
    static member CreateStaticMember(binding:SynBindingRcd) =
        let (SynValData(usedMemberFlags, valInfo, identifier)) = binding.ValData
        let staticMemberFlags = Some {
            // this means the member is static
            IsInstance = false;
            IsOverrideOrExplicitImpl = false
            IsDispatchSlot = false;
            IsFinal = false
            MemberKind = MemberKind.Member
        }
        let staticBinding = { binding with ValData = SynValData.SynValData(staticMemberFlags, valInfo, identifier) }
        SynMemberDefn.Member(staticBinding.FromRcd, range.Zero)

type SynAttribute with
    static member Create(idents: string list) : SynAttribute =
        {
            AppliesToGetterAndSetter = false
            ArgExpr = SynExpr.Const (SynConst.Unit, range0)
            Range = range0
            Target = None
            TypeName = LongIdentWithDots(List.map Ident.Create idents, [ ])
        }

type SynType with
    static member KeyValuePair(keyType, valueType) =
        SynType.App(
            typeName=SynType.Create "KeyValuePair",
            typeArgs=[ keyType; valueType ],
            commaRanges = [ ],
            isPostfix = false,
            range=range0,
            greaterRange=None,
            lessRange=None
        )

    static member ByteArray() = SynType.Array(1, SynType.Byte(), range0)

open System
open System.Xml.Linq

[<AbstractClass; Sealed>]
type XAttribute =

    static member ofStringName (name: string, value: obj) =
        XAttribute(XName.Get(name), value)

[<AbstractClass; Sealed>]
type XElement =

    static member ofStringName (name: string, content: obj) =
        XElement(XName.Get(name), content)

    static member ofStringName (name: string, [<ParamArray>] content) =
        XElement(XName.Get(name), content)

    static member Compile(fileName: string) =
        XElement.ofStringName("Compile", XAttribute.ofStringName("Include", fileName))

    static member PackageReference (include': string, version: string) =
        XElement.ofStringName("PackageReference",
            XAttribute.ofStringName("Include", include'),
            XAttribute.ofStringName("Version", version))

    static member ProjectReference (include': string) =
        XElement.ofStringName("ProjectReference",
            XAttribute.ofStringName("Include", include'))