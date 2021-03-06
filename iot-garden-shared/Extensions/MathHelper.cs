using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iot_garden_shared.Extensions
{
    /// <summary>
    /// Add extended math utils.
    /// </summary>
    public static class MathHelper
    {
        /// <summary>
        /// PI x 2 value.
        /// </summary>
        public static readonly float PI2 = (float)Math.PI * 2;

        /// <summary>
        /// Convert degrees to radians.
        /// </summary>
        public static float ToRadians(this float degrees)
        {
            return degrees * (float)Math.PI / 180;
        }

        /// <summary>
        /// Convert radians to degrees.
        /// </summary>
        public static float ToDegrees(this float radians)
        {
            return radians * 180f / (float)Math.PI;
        }

        /// <summary>
        /// Find shortest distance between two given radians.
        /// </summary>
        public static float RadiansDistance(float a0, float a1)
        {
            float max = (float)Math.PI * 2f;
            float da = (a1 - a0) % max;
            return 2f * (da % max) - da;
        }

        /// <summary>
        /// Find shortest distance between two given angles.
        /// </summary>
        public static float DegreesDistance(float a0, float a1, bool wrap)
        {
            float distance = RadiansDistance(ToRadians(a0), ToRadians(a1));
            float ret = ToDegrees(distance);
            if (wrap) { ret = WrapAngle(ret); }
            return ret;
        }

        /// <summary>
        /// Lerp between two radian angles.
        /// </summary>
        public static float LerpRadians(float a0, float a1, float alpha)
        {
            var distance = RadiansDistance(a0, a1);
            return a0 + distance * alpha;
        }


        /// <summary>
        /// Lerp between two degrees.
        /// </summary>
        public static float LerpDegrees(float a0, float a1, float alpha)
        {
            // convert to radians
            a0 = ToRadians(a0);
            a1 = ToRadians(a1);

            // lerp
            var ret = LerpRadians(a0, a1, alpha);

            // convert back to degree and return
            return ToDegrees(ret);
        }

        /// <summary>
        /// Return angle (in radians) between two points.
        /// </summary>
        public static float AngleBetween(float x1, float y1, float x2, float y2, bool wrap)
        {
            float deltaY = y2 - y1;
            float deltaX = x2 - x1;
            var ret = (float)Math.Atan2(deltaY, deltaX);
            if (wrap) { while (ret < 0) ret += PI2; }
            return ret;
        }

        /// <summary>
        /// Wrap angle to be between 0 and 360.
        /// </summary>
        public static float WrapAngle(float angle)
        {
            // return Math.Abs(angle % 360);
            while (angle > 360) { angle -= 360; }
            while (angle < 0) { angle += 360; }
            return angle;
        }
    }
}
