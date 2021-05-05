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