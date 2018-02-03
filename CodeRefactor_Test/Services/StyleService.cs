using CodeRefactor_Test.BusinessLogic;
using CodeRefactor_Test.Models.GraphicsModel;
using CodeRefactor_Test.Models.ProbabilityModel;

namespace CodeRefactor_Test.Services
{
    public class StyleService
    {
        public static Color DefaultFilledStyleFromProbability(ResponseFeature feature)
        {
            //Get the color based on the method
            Color fillColor;
            switch (feature.method)
            {
                case "UND":
                    fillColor = ColorPicker.DefaultColorOne(feature.probability);
                    break;
                case "MS":
                    fillColor = ColorPicker.DefaultColorTwo(feature.probability);
                    break;
                case "DS":
                    fillColor = ColorPicker.DefaultColorThree(feature.probability);
                    break;
                case "DNE":
                    fillColor = ColorPicker.DefaultColorFour(0);
                    break;
                default:
                    fillColor = ColorPicker.DefaultColorFour(0);
                    break;
            }
            return fillColor;
        }
    }
}