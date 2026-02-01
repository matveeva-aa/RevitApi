using CSharpFunctionalExtensions;
using Task_10.Models;

namespace Task_10.Abstractions
{
    public interface IPlacementService
    {
        Result Place(TreeType selectedTreeType, int count);
    }
}
