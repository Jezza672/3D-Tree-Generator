﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Tree_Generator
{
    public static class ExtensionMethods //https://stackoverflow.com/questions/14353485/how-do-i-map-numbers-in-c-sharp-like-with-map-in-arduino
    {
        public static float Lerp(this float value, double fromSource, double toSource, double fromTarget, double toTarget)
        {
            return (float)((value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget);
        }

        public static float InterpRoot(this float value, double fromSource, double toSource, double fromTarget, double toTarget)
        {
            value = (float)Math.Sqrt(value);
            fromSource = Math.Sqrt(fromSource);
            toSource = Math.Sqrt(toSource);
            return (float)( (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget);
        }

        public static float InterpCust(this float value, double fromSource, double toSource, double fromTarget, double toTarget, Func<double, float> function)
        {
            value = function(value);
            fromSource = function(fromSource);
            toSource = function(toSource);
            return (float)((value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget);
        }


        public static float CustInterp(this float value, double fromSource, double toSource, double fromTarget, double toTarget, Func<double, float> f, Func<double, float> i)
        {
            value = f(value);
            fromSource = f(fromSource);
            toSource = f(toSource);
            return i(((value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget));
        }

    }
}
