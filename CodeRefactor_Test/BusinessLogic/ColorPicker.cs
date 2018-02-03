using CodeRefactor_Test.Models.GraphicsModel;

namespace CodeRefactor_Test.BusinessLogic
{
    public class ColorPicker
    {
        static double max = 0.7913012;
        //Orange
        public static Color DefaultColorOne(double probability)
        {
            Color Orange = new Color();

            if ((229 - (max - probability) / (max - 0.2) * (229 - 255)) > 255)
                Orange.Red = 255;
            else
                Orange.Red = (byte)(229 - (max - probability) / (max - 0.2) * (229 - 255));
            //########
            if ((119 - (max - probability) / (max - 0.2) * (119 - 190)) > 255)
                Orange.Green = 255;
            else
                Orange.Green = (byte)(119 - (max - probability) / (max - 0.2) * (119 - 190));
            //########
            if ((0 - (max - probability) / (max - 0.2) * (0 - 117)) > 255)
                Orange.Blue = 255;
            else
                Orange.Blue = (byte)(0 - (max - probability) / (max - 0.2) * (0 - 117));

            return Orange;
        }
        
        //Green
        public static Color DefaultColorTwo(double probability)
        {
            Color Green = new Color();

            if ((0 - (max - probability) / (max - 0.2) * (-189)) > 255)
                Green.Red = 255;
            else
                Green.Red= (byte)(0 - (max - probability) / (max - 0.2) * (-189));
            //########
            if ((71 - (max - probability) / (max - 0.2) * (71 - 255)) > 255)
                Green.Green = 255;
            else
                Green.Green = (byte)(71 - (max - probability) / (max - 0.2) * (71 - 255));
            //########
            if ((5 - (max - probability) / (max - 0.2) * (5 - 193)) > 255)
                Green.Blue = 255;
            else
                Green.Blue = (byte)(5 - (max - probability) / (max - 0.2) * (5 - 193));

            return Green;
        }
        
        //Red
        public static Color DefaultColorThree(double probability)
        {
            Color Red = new Color(); 

            if ((112 - (max - probability) / (max - 0.2) * (112 - 255)) > 255)
                Red.Red = 255;
            else
                Red.Red = (byte)(112 - (max - probability) / (max - 0.2) * (112 - 255));
            //########
            if ((4 - (max - probability) / (max - 0.2) * (4 - 221)) > 255)
                Red.Green = 255;
            else
                Red.Green = (byte)(4 - (max - probability) / (max - 0.2) * (4 - 221));
            //########
            if ((0 - (max - probability) / (max - 0.2) * (0 - 219)) > 255)
                Red.Blue = 255;
            else
                Red.Blue = (byte)(0 - (max - probability) / (max - 0.2) * (0 - 219));
            return Red;
        }
        
        //Grey
        public static Color DefaultColorFour(double probability)
        {
            Color Grey = new Color();
            Grey.Red = 150;
            Grey.Green = 150;
            Grey.Blue = 150;
            return Grey;
        }
    }
}