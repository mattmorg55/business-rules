namespace AJBoggs.ADAP.BR.Core.Rules
{
    public interface ICreateFormDecisionRow
    {
        bool? CanCreate { get; set; }
        FunctionColumn CheckReenrollmentWindow { get; set; }
        FunctionColumn CheckSvfWindow { get; set; }
        string Explanation { get; set; }
        string Form { get; set; }
        string InitialEnrollmentStatus { get; set; }
        string ReenrollmentStatus { get; set; }
        string SvfNoChangesStatus { get; set; }
        string SvfWithChangesStatus { get; set; }
        string UpdateFormStatus { get; set; }

        string ToString();
    }
}