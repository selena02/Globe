namespace Application.Common.Models;

public class LandmarkDetectorResponse
{
    public string Error { get; set; }
    public string Name { get; set; }
    public double Score { get; set; }
}