using UTTraining.Infrastructure;
using UTTraining.Model;

namespace UTTraining.Interface
{
    public interface ISageIntegrationLogic
    {
        LogicResult<ProcessedData> ProcessFormData(Invoice data);
    }
}
