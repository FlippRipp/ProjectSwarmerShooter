using UnityEngine;

namespace FG
{
    public class OrbitalCamera : MonoBehaviour
    {
        [SerializeField] private Transform focus;
        [SerializeField, Range(1f, 20f)] private float maxDistanceFromPlayer = 2f, minDistanceFromPlayer = 10f;
        [SerializeField, Range(1f, 10f)] private float zoomSensitivity = 3f;
        [SerializeField, Min(0f)] private float focusRadius = 1f;
        [SerializeField, Range(0f, 1f)] private float focusCentering = 0.5f;
        [SerializeField, Range(1f, 360f)] private float rotationSpeed = 90f;
        [SerializeField, Range(-90, 90)] private float minVerticalAngle = -30f, maxVerticalAngle = 60f;
        [SerializeField, Range(1f, 10f)] private float sensitivity = 3;
        [SerializeField, Min(0F)] private float alignDelay = 5f;
        [SerializeField, Range(0f, 90f)] private float alignSmoothRange = 45f;
        [SerializeField] private LayerMask cameraIgnoreLayer;
        [SerializeField] private Vector3 offset;

        Vector2 orbitAngles = new Vector2(80f, 0);

        private float currentDistanceFromPlayer;

        private Vector2 input;

        private bool cameraControlsEnabled = true;

        private Vector3 focusPoint, lastFocusPoint;
        private float lastManualRotationTime;
        
        private void Awake()
        {
            currentDistanceFromPlayer = minDistanceFromPlayer;
            focusPoint = focus.position + offset;
            transform.localRotation = Quaternion.Euler(orbitAngles);
            GameplayEventManager.instance.OnEndGame += DisableCamera;
        }

        private void OnValidate()
        {
            if (maxVerticalAngle < minVerticalAngle)
            {
                maxVerticalAngle = minVerticalAngle;
            }
            if (maxDistanceFromPlayer < minDistanceFromPlayer)
            {
                maxDistanceFromPlayer = minDistanceFromPlayer;
            }
        }

        private void DisableCamera()
        {
            cameraControlsEnabled = false;
        }

        private void Update()
        {
            if(!cameraControlsEnabled) return;
            currentDistanceFromPlayer -= Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity;
            if (currentDistanceFromPlayer > maxDistanceFromPlayer)
            {
                currentDistanceFromPlayer = maxDistanceFromPlayer;
            }
            else if(currentDistanceFromPlayer < minDistanceFromPlayer)
            {
                currentDistanceFromPlayer = minDistanceFromPlayer;
            }
        }

        private void LateUpdate()
        {
            if(!cameraControlsEnabled) return;
            UpdateFocusPoint();
            ManualRotation();
            Quaternion lookRotation;

            if (ManualRotation() || AutoRotation())
            {
                ConstrainAngle();
                lookRotation = Quaternion.Euler(orbitAngles);
            }
            else
            {
                lookRotation = transform.localRotation;
            }

            Vector3 lookDirection = lookRotation * Vector3.forward;
            Vector3 lookPosition = focusPoint - lookDirection * currentDistanceFromPlayer;
            if (Physics.Raycast(focusPoint, -lookDirection, out RaycastHit hit, currentDistanceFromPlayer * 1.1f, cameraIgnoreLayer))
            {
                lookPosition = focusPoint - lookDirection * (hit.distance * 0.9f);
            }
            transform.SetPositionAndRotation(lookPosition, lookRotation);
        }

        private void UpdateFocusPoint()
        {
            lastFocusPoint = focusPoint;
            Vector3 targetPosition = focus.TransformPoint(offset);
            if (focusRadius > 0f)
            {
                float distance = Vector3.Distance(targetPosition, focusPoint);
                float t = 1f;
                if (distance > 0.01f && focusCentering > 0f)
                {
                    t = Mathf.Pow(1f - focusCentering, Time.unscaledDeltaTime);
                }

                if (distance > focusRadius)
                {
                    t = Mathf.Min(t, focusRadius / distance);
                }

                focusPoint = Vector3.Lerp(targetPosition, focusPoint, t);
            }
            else
            {
                focusPoint = targetPosition;
            }
        }

        private bool ManualRotation()
        {
            input = new Vector2(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));

            float e = Mathf.Epsilon;
            if (!(Mathf.Abs(input.x) > e) && !(Mathf.Abs(input.y) > e)) return false;
            orbitAngles += rotationSpeed * Time.unscaledDeltaTime * sensitivity * input;
            lastManualRotationTime = Time.unscaledTime;
            return true;
        }

        private bool AutoRotation()
        {
            if (Time.unscaledTime - lastManualRotationTime < alignDelay)
            {
                return false;
            }

            Vector2 movement = new Vector2(focusPoint.x - lastFocusPoint.x, focusPoint.z - lastFocusPoint.z);
            float movementDeltaSqr = movement.sqrMagnitude;
            if (movementDeltaSqr < 0.000001f)
            {
                return false;
            }

            float headingAngle = GetAngle(movement / Mathf.Sqrt(movementDeltaSqr));
            float deltaAbs = Mathf.Abs(Mathf.DeltaAngle(orbitAngles.y, headingAngle));
            float rotationChange = rotationSpeed * Mathf.Min(Time.unscaledDeltaTime, movementDeltaSqr);
            if (deltaAbs < alignSmoothRange)
            {
                rotationChange *= deltaAbs / alignSmoothRange;
            }
            else if (180f - deltaAbs < alignSmoothRange)
            {
                rotationChange *= (180f - deltaAbs) / alignSmoothRange;
            }

            orbitAngles.y = Mathf.MoveTowardsAngle(orbitAngles.y, headingAngle, rotationChange);
            return true;
        }

        private static float GetAngle(Vector3 direction)
        {
            float angle = Mathf.Acos(direction.y) * Mathf.Rad2Deg;
            return direction.x < 0f ? 360 - angle : angle;
        }

        private void ConstrainAngle()
        {
            orbitAngles.x = Mathf.Clamp(orbitAngles.x, minVerticalAngle, maxVerticalAngle);

            if (orbitAngles.y < 0f)
            {
                orbitAngles.y += 360f;
            }
            else if (orbitAngles.y >= 360f)
            {
                orbitAngles.y -= 360f;
            }
        }
    }
}