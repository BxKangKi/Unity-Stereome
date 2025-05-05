using UnityEngine;
using Unity.Mathematics;

namespace Stereome
{
    /// <summary>
    /// JMath is made for using methods from UnityEngine.Mathf at Job System.
    /// </summary>
    public readonly struct JMath
    {
        // Degrees-to-radians conversion constant (Read Only).
        public const float Deg2Rad = math.PI / 180f;
        // Radians-to-degrees conversion constant (Read Only).
        public const float Rad2Deg = 180f / math.PI;
        public const float Epsilon = 0.00001f;
        private const float kEpsilonNormalSqrt = 1e-15f;

        public static float Lerp(float a, float b, float t, float m)
        {
            return math.lerp(a, b, math.saturate(t * m));
        }

        public static float Lerp(float Current, float Desired, float IncreaseSpeed, float DecreaseSpeed, float Velocity, float DeltaTime)
        {
            if (Current == Desired)
                return Desired;
            if (Current < Desired)
                return MoveTowards(Current, Desired, (IncreaseSpeed * Velocity) * DeltaTime);
            else
                return MoveTowards(Current, Desired, (DecreaseSpeed * Velocity) * DeltaTime);
        }

        public static float MoveTowards(float current, float target, float maxDelta)
        {
            if (math.abs(target - current) <= maxDelta)
            {
                return target;
            }
            return current + math.sign(target - current) * maxDelta;
        }


        public static float3 ToEulerRad(Quaternion q)
        {
            float3 eulerAngles = float3.zero;

            // Roll (Z-axis rotation)
            float sinRCosP = 2.0f * (q.w * q.z + q.x * q.y);
            float cosRCosP = 1.0f - 2.0f * (q.y * q.y + q.z * q.z);
            eulerAngles.z = math.atan2(sinRCosP, cosRCosP);

            // Pitch (X-axis rotation)
            float sinP = 2.0f * (q.w * q.x - q.z * q.y);
            if (math.abs(sinP) >= 1.0f)
                eulerAngles.x = math.sign(sinP) * (math.PI / 2.0f); // Clamp to 90 degrees
            else
                eulerAngles.x = math.asin(sinP);

            // Yaw (Y-axis rotation)
            float sinYCosP = 2.0f * (q.w * q.y + q.z * q.x);
            float cosYCosP = 1.0f - 2.0f * (q.x * q.x + q.y * q.y);
            eulerAngles.y = math.atan2(sinYCosP, cosYCosP);
            return eulerAngles;
        }

        public static float Repeat(float t, float length)
        {
            if (length == 0f)
            {
                length = 0.00001f;
            }
            return math.clamp(t - math.floor(t / length) * length, 0.0f, length);
        }

        public static float DeltaAngle(float current, float target)
        {
            float delta = Repeat((target - current), 360.0F);
            if (delta > 180.0F)
            {
                delta -= 360.0F;
            }
            return delta;
        }


        public static float LerpThreshold(float a, float b, float t, float threshold)
        {
            float result = math.lerp(a, b, t);
            if (math.abs(result) < math.abs(threshold))
            {
                result = b;
            }
            return result;
        }


        public static float3 LerpThreshold(float3 a, float3 b, float t, float threshold)
        {
            float3 result = new float3()
            {
                x = LerpThreshold(a.x, b.x, t, threshold),
                y = LerpThreshold(a.y, b.y, t, threshold),
                z = LerpThreshold(a.z, b.z, t, threshold)
            };
            return result;
        }


        /// <summary>
        /// Gradually changes an angle given in degrees towards a desired goal angle over time.<br />
        /// This method smoothes the value with a spring-damper like algorithm that never overshoots the target value. 
        /// This algorithm is based on Game Programming Gems 4, Chapter 1.10.<br />
        /// Note: This method attempts to avoid overshooting the target value. When deltaTime is 0.0f, 
        /// this yields NaN for the currentVelocity. 
        /// If you call back with a currentVelocity of NaN, this method returns a NaN.<br />
        /// https://docs.unity3d.com/ScriptReference/Mathf.SmoothDampAngle.html<br />
        /// </summary>
        /// <param name="current">The current position.</param>
        /// <param name="target">The target position.</param>
        /// <param name="currentVelocity">The current velocity. 
        /// This method modifies the currentVelocity every time the method is called.</param>
        /// <param name="smoothTime">The approximate time it takes to reach the target position. 
        /// The lower the value the faster this method reaches the target. 
        /// The minimum value is 0.0001. 
        /// If a lower value is specified, it is automatically clamped to this minimum value.</param>
        /// <param name="deltaTime">The time since this method was last called.</param>
        /// <param name="maxSpeed">	Use this optional parameter to specify a maximum speed. 
        /// By default, the maximum speed is set to infinity.</param>
        /// <returns></returns>
        public static float SmoothDampAngle(float current, float target, ref float currentVelocity, float smoothTime, float deltaTime, float maxSpeed = float.MaxValue)
        {
            target = current + DeltaAngle(current, target);
            return SmoothDamp(current, target, ref currentVelocity, smoothTime, deltaTime, maxSpeed);
        }


        /// <summary>
        /// Gradually changes a value towards a desired goal over time.<br />
        /// This method smoothes the current value towards a target value with a spring-damper like algorithm.
        /// This algorithm is based on Game Programming Gems 4, Chapter 1.10.<br />
        /// Note: This method attempts to avoid overshooting the target value.When deltaTime is 0.0f, 
        /// this yields NaN for the currentVelocity. 
        /// If you call back with a currentVelocity of NaN, this method returns a NaN.<br />
        /// </summary>
        /// <param name="current">The current value.</param>
        /// <param name="target">The target value.</param>
        /// <param name="currentVelocity">Use this parameter to specify the initial velocity 
        /// to move the current value towards the target value. 
        /// This method updates the currentVelocity based on this movement and smooth-damping.</param>
        /// <param name="smoothTime">The approximate time it takes for the current value to reach the target value. 
        /// The lower the smoothTime, the faster the current value reaches the target value. 
        /// The minimum smoothTime is 0.0001. 
        /// If a lower value is specified, it is clamped to the minimum value.</param>
        /// <param name="deltaTime">The time since this method was last called.</param>
        /// <param name="maxSpeed">	Use this optional parameter to specify a maximum speed. 
        /// By default, the maximum speed is set to infinity.</param>
        /// <returns>float The current value after moving one step towards the target value.</returns>
        public static float SmoothDamp(float current, float target, ref float currentVelocity, float smoothTime, float deltaTime, float maxSpeed = float.MaxValue)
        {
            // Based on Game Programming Gems 4 Chapter 1.10
            smoothTime = math.max(0.00001f, smoothTime);
            float omega = 2F / smoothTime;

            float x = omega * deltaTime;
            float exp = 1F / (1F + x + 0.48f * x * x + 0.235f * x * x * x);
            float change = current - target;
            float originalTo = target;

            // Clamp maximum speed
            float maxChange = maxSpeed * smoothTime;
            change = math.clamp(change, -maxChange, maxChange);
            target = current - change;

            float temp = (currentVelocity + omega * change) * deltaTime;
            currentVelocity = (currentVelocity - omega * temp) * exp;
            float output = target + (change + temp) * exp;

            // Prevent overshooting
            if (originalTo - current > 0.0f == output > originalTo)
            {
                output = originalTo;
                currentVelocity = (output - originalTo) / math.max(0.00001f, deltaTime);
            }

            return output;
        }


        public static float ClampAngle(float value, float min, float max)
        {
            if (value < -360f) value += 360f;
            if (value > 360f) value -= 360f;
            value = (value > max) ? max : ((value < min) ? min : value);
            return value;

        }

        /// <summary>
        /// Calculate angle between two float3.<br />
        /// The angle returned is the angle of rotation from the first float3 to the second, 
        /// when treating these two float3 inputs as directions.
        /// Note: The angle returned will always be between 0 and 180 degrees, 
        /// because the method returns the smallest angle between the float3. 
        /// That is, it will never return a reflex angle.<br />
        /// https://docs.unity3d.com/Documentation/ScriptReference/Vector3.Angle.html<br />
        /// </summary>
        /// <param name="from">The float3 from which the angular difference is measured.</param>
        /// <param name="to">The float3 to which the angular difference is measured.</param>
        /// <returns>(float) The angle in degrees between the two vectors.</returns>
        public static float Angle(float3 from, float3 to)
        {
            // sqrt(a) * sqrt(b) = sqrt(a * b) -- valid for real numbers
            float denominator = (float)math.sqrt(math.lengthsq(from) * math.lengthsq(to));
            if (denominator < kEpsilonNormalSqrt)
            {
                return 0f;
            }
            float dot = math.clamp(math.dot(from, to) / denominator, -1f, 1f);
            return ((float)math.acos(dot)) * Rad2Deg;
        }


        public static float3 ProjectOnPlane(float3 vector, float3 planeNormal)
        {
            float sqrMag = math.dot(planeNormal, planeNormal);
            if (sqrMag < Epsilon)
            {
                return vector;
            }
            else
            {
                var dot = math.dot(vector, planeNormal);
                return new float3(vector.x - planeNormal.x * dot / sqrMag,
                    vector.y - planeNormal.y * dot / sqrMag,
                    vector.z - planeNormal.z * dot / sqrMag);
            }
        }

        public static float Vector3Difference(Vector3 a, Vector3 b)
        {
            return math.abs(a.x - b.x) + math.abs(a.y - b.y) + math.abs(a.z - b.z) * 0.33333f;
        }

        /// <summary>
        /// Creates a rotation from fromDirection to toDirection.<br />
        /// Use this method to rotate a transform so that one of its axes, such as the y-axis,
        /// follows the target direction, toDirection, in world space.<br />
        /// https://docs.unity3d.com/Documentation/ScriptReference/Mathf.SmoothDamp.html<br />
        /// </summary>
        /// <param name="fromDirection">A non-unit or unit vector representing a direction axis to rotate.</param>
        /// <param name="toDirection">A non-unit or unit vector representing the target direction axis.</param>
        /// <returns>Quaternion A unit quaternion which rotates from fromDirection to toDirection.</returns>
        public static quaternion FromToRotation(float3 fromDirection, float3 toDirection)
        {
            // 두 벡터를 정규화
            fromDirection = math.normalize(fromDirection);
            toDirection = math.normalize(toDirection);

            // 두 벡터 사이의 회전축 계산
            float3 rotationAxis = math.cross(fromDirection, toDirection);
            float sinAngle = math.length(rotationAxis);

            // 두 벡터 사이의 각도 계산
            float cosAngle = math.dot(fromDirection, toDirection);

            // 두 벡터가 거의 동일한 경우 (회전 필요 없음)
            if (sinAngle < 1e-6f)
            {
                // 두 벡터가 동일한 경우
                if (cosAngle > 0.9999f)
                    return quaternion.identity;
                // 두 벡터가 정반대인 경우
                else
                    return new quaternion(0f, 0f, 1f, 0f); // 180도 회전 (임의의 축을 기준으로)
            }

            // 쿼터니언 생성
            float angle = math.atan2(sinAngle, cosAngle); // 회전각
            float halfAngle = angle * 0.5f;
            float sinHalfAngle = math.sin(halfAngle);
            quaternion rotation = new quaternion(rotationAxis.x * sinHalfAngle, rotationAxis.y * sinHalfAngle, rotationAxis.z * sinHalfAngle, math.cos(halfAngle));
            return rotation;
        }


        /// <summary>
        /// add two quaternion
        /// </summary>
        /// <param name="a">first quaternion</param>
        /// <param name="b">second quaternion</param>
        /// <returns>added quaternion</returns>
        public static quaternion AddRotation(quaternion a, quaternion b)
        {
            float4 lhs = a.value;
            float4 rhs = b.value;
            return new quaternion(lhs.w * rhs.x + lhs.x * rhs.w + lhs.y * rhs.z - lhs.z * rhs.y,
                                  lhs.w * rhs.y + lhs.y * rhs.w + lhs.z * rhs.x - lhs.x * rhs.z,
                                  lhs.w * rhs.z + lhs.z * rhs.w + lhs.x * rhs.y - lhs.y * rhs.x,
                                  lhs.w * rhs.w - lhs.x * rhs.x - lhs.y * rhs.y - lhs.z * rhs.z);
        }
    }
}