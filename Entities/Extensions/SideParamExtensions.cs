using Entities.Exceptions;

namespace Entities.Extensions
{
    public static class SideParamExtensions
    {
        public static bool GetSideBool(this string sideString)
        {
            bool side;

            if (sideString.Equals("left"))
                side = false;
            else if (sideString.Equals("right"))
                side = true;
            else
                throw new SideParameterBadRequestException();

            return side;
        }

        public static string GetSideString(this bool sideBool)
        {
            string side;

            if (!sideBool)
                side = "left";
            else
                side = "right";

            return side;
        }
    }
}
