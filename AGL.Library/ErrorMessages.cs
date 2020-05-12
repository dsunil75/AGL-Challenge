using System;
using System.Collections.Generic;
using System.Text;

namespace AGL.Library
{
    public static class ErrorMessages
    {
        public static string CannotConnectToServer_01 => "Unable to connect to server. Please try again later.";
        public static string CannotDeserializePeople_02 => "Unable to deserialize the People List. The schema/structure for the People List has changed.";
    }
}
