using System.ComponentModel;

namespace Helper.Enum
{
    public enum EAttribute
    {
        [Description("SER_cad_Cpf")]
        SER_cad_Cpf = 1,
        [Description("SER_cad_Curso")]
        SER_cad_Curso = 2,
        [Description("SER_cad_Matricula")]
        SER_cad_Matricula = 3,
        [Description("SER_cad_NomedoAluno")]
        SER_cad_NomedoAluno = 4,
        [Description("SER_cad_SituacaoAluno")]
        SER_cad_SituacaoAluno = 5,
        [Description("SER_cad_Unidade")]
        SER_cad_Unidade = 6,
        [Description("SER_EstagioDoc")]
        SER_EstagioDoc = 7,
        [Description("SER_Input_Compl")]
        SER_Input_Compl = 8,
        [Description("SER_Input_DataRef")]
        SER_Input_DataRef = 9,
        [Description("SER_Input_Data_Vencto")]
        SER_Input_Data_Vencto = 10,
        [Description("SER_Input_NumDoc")]
        SER_Input_NumDoc = 11,
        [Description("SER_Operador")]
        SER_Operador = 12,
        [Description("SER_Paginas")]
        SER_Paginas = 13,
        [Description("SER_SituacaoDoc")]
        SER_SituacaoDoc = 14,
        [Description("SER_cad_cod_unidade")]
        SER_cad_cod_unidade = 15,

        [Description("MFP_Data_Job")]
        MFP_Data_Job = 16,
        [Description("MFP_Status")]
        MFP_Status = 17,
        [Description("MFP_Categoria")]
        MFP_Categoria = 18,
        [Description("MFP_Usuario")]
        MFP_Usuario = 19,
    }
}