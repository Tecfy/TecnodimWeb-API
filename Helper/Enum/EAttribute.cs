using System.ComponentModel;

namespace Helper.Enum
{
    public enum EAttribute
    {
        [Description("SER_NomedoAluno")]
        SER_NomedoAluno = 1,
        [Description("SER_Matricula")]
        SER_Matricula = 2,
        [Description("SER_CPF")]
        SER_CPF = 3,
        [Description("SER_CURSO")]
        SER_CURSO = 4,
        [Description("SER_Unidade")]
        SER_Unidade = 5,
        [Description("SER_SituacaoDoc")]
        SER_SituacaoDoc = 6,
    }
}