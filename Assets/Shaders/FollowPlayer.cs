using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset;
    ParticleSystem.Particle[] mCurrentParticles_ForManualUpdate;
    [SerializeField] private ParticleSystem mParticlesystem;
    [SerializeField] private Camera cCamera;
    [SerializeField] private bool mPerformCameraWrapping = true;

    private void LateUpdate()
    {
        if (mParticlesystem == null) return;

        // Check
        if (mPerformCameraWrapping && cCamera != null)
        {
            // Ensure mCurrentParticles_ForManualUpdate has correct size
            if (mCurrentParticles_ForManualUpdate == null || mCurrentParticles_ForManualUpdate.Length < mParticlesystem.main.maxParticles)
                mCurrentParticles_ForManualUpdate = new ParticleSystem.Particle[mParticlesystem.main.maxParticles];

            // GetParticles is allocation free because we reuse the m_Particles buffer between updates
            int numParticlesAlive = mParticlesystem.GetParticles(mCurrentParticles_ForManualUpdate);

            // Calc
            Vector3 camPos = cCamera.transform.position;
            float OOBPercMult = 1.3f;
            float OOBPercMult_Smaller = 1.25f; // When wrapping, use this value so it doesn't immediately become OOB again on the other side

            // X
            float screenHalfWidth = Screen.width / 2;
            float screenHalfHeight = Screen.height / 2;
            float OOB_Left = camPos.x - (screenHalfWidth * OOBPercMult);
            float OOB_Right = camPos.x + (screenHalfWidth * OOBPercMult);
            float modAmountX = Screen.width * OOBPercMult_Smaller;

            // Y
            float OOB_Top = camPos.y + (screenHalfHeight * OOBPercMult);
            float OOB_Bottom = camPos.y - (screenHalfHeight * OOBPercMult);
            float modAmountY = Screen.height * OOBPercMult_Smaller;

            // Iterate through alive particles
            for (int i = 0; i < numParticlesAlive; i++)
            {
                // Check in X
                if (mCurrentParticles_ForManualUpdate[i].position.x > OOB_Right)
                {
                    // Move
                    mCurrentParticles_ForManualUpdate[i].position = new Vector3(mCurrentParticles_ForManualUpdate[i].position.x - modAmountX, mCurrentParticles_ForManualUpdate[i].position.y, mCurrentParticles_ForManualUpdate[i].position.z);
                }
                else if (mCurrentParticles_ForManualUpdate[i].position.x < OOB_Left)
                {
                    // Move
                    mCurrentParticles_ForManualUpdate[i].position = new Vector3(mCurrentParticles_ForManualUpdate[i].position.x + modAmountX, mCurrentParticles_ForManualUpdate[i].position.y, mCurrentParticles_ForManualUpdate[i].position.z);
                }

                // Check in Y
                if (mCurrentParticles_ForManualUpdate[i].position.y > OOB_Top)
                {
                    // Move
                    mCurrentParticles_ForManualUpdate[i].position = new Vector3(mCurrentParticles_ForManualUpdate[i].position.x, mCurrentParticles_ForManualUpdate[i].position.y - modAmountY, mCurrentParticles_ForManualUpdate[i].position.z);
                }
                else if (mCurrentParticles_ForManualUpdate[i].position.y < OOB_Bottom)
                {
                    // Move
                    mCurrentParticles_ForManualUpdate[i].position = new Vector3(mCurrentParticles_ForManualUpdate[i].position.x, mCurrentParticles_ForManualUpdate[i].position.y + modAmountY, mCurrentParticles_ForManualUpdate[i].position.z);
                }
            }

            // Apply the particle changes to the Particle System
            mParticlesystem.SetParticles(mCurrentParticles_ForManualUpdate, numParticlesAlive);
        }
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y, player.position.z + offset.z);
    }
}
