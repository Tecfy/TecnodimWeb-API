using System.ComponentModel;

namespace Helper.Enum
{
    public enum EAttribute
    {
        [Description("SER_cad_Cpf")]
        SER_cad_Cpf = 1,
        [Description("SER_cad_RG")]
        SER_cad_RG = 2,
        [Description("SER_cad_Curso")]
        SER_cad_Curso = 3,
        [Description("SER_cad_Matricula")]
        SER_cad_Matricula = 4,
        [Description("SER_cad_NomedoAluno")]
        SER_cad_NomedoAluno = 5,
        [Description("SER_cad_SituacaoAluno")]
        SER_cad_SituacaoAluno = 6,
        [Description("SER_cad_Unidade")]
        SER_cad_Unidade = 7,
        [Description("SER_cad_unidades")]
        SER_cad_unidades = 8,
        [Description("SER_EstagioDoc")]
        SER_EstagioDoc = 9,
        [Description("SER_Input_Compl")]
        SER_Input_Compl = 10,
        [Description("SER_Input_DataRef")]
        SER_Input_DataRef = 11,
        [Description("SER_Input_Data_Vencto")]
        SER_Input_Data_Vencto = 12,
        [Description("SER_Input_NumDoc")]
        SER_Input_NumDoc = 13,
        [Description("SER_Operador")]
        SER_Operador = 14,
        [Description("SER_Paginas")]
        SER_Paginas = 15,
        [Description("SER_SituacaoDoc")]
        SER_SituacaoDoc = 16,
        [Description("SER_cad_cod_unidade")]
        SER_cad_cod_unidade = 17,

        [Description("MFP_Data_Job")]
        MFP_Data_Job = 18,
        [Description("MFP_Status")]
        MFP_Status = 19,
        [Description("MFP_Categoria")]
        MFP_Categoria = 20,
        [Description("MFP_Usuario")]
        MFP_Usuario = 21,

        [Description("SER_id_classificador")]
        SER_id_classificador = 22,
        [Description("SER_id_recortador")]
        SER_id_recortador = 23,
        [Description("SER_nome_classificador")]
        SER_nome_classificador = 24,
        [Description("SER_nome_recortador")]
        SER_nome_recortador = 25,
    }
}