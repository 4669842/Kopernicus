/**
 * Kopernicus Planetary System Modifier
 * ------------------------------------------------------------- 
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 3 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston,
 * MA 02110-1301  USA
 * 
 * This library is intended to be used as a plugin for Kerbal Space Program
 * which is copyright 2011-2017 Squad. Your usage of Kerbal Space Program
 * itself is governed by the terms of its EULA, not the license above.
 * 
 * https://kerbalspaceprogram.com
 */
 
using System;
using System.Linq;
using Kopernicus.OnDemand;
using UnityEngine;

namespace Kopernicus
{
    [KSPAddon(KSPAddon.Startup.MainMenu, false)]
    public class MainMenuFixer : MonoBehaviour
    {
        void Awake()
        {
            if (Templates.kopernicusMainMenu)
            {
                // Get the MainMenu-Logic
                MainMenu main = FindObjectOfType<MainMenu>();
                if (main == null)
                {
                    Debug.LogError("[SigmaLog] No main menu object!");
                    return;
                }
                MainMenuEnvLogic logic = main.envLogic;

                // Set it to Space, because the Mun-Area won't work with sth else than Mun
                if (logic.areas.Length < 2)
                {
                    Debug.LogError("[SigmaLog] Not enough scenes");
                    return;
                }

                logic.areas[0].SetActive(false);
                logic.areas[1].SetActive(true);
            }
        }

        // Update the menu body
        void Start()
        {
            if (Templates.kopernicusMainMenu)
            {
                // Select a random body?
                if (Templates.randomMainMenuBodies.Any())
                {
                    Templates.menuBody =
                        Templates.randomMainMenuBodies[new System.Random().Next(0, Templates.randomMainMenuBodies.Count)];
                }

                // Grab the main body
                CelestialBody planetCB = PSystemManager.Instance.localBodies.Find(b => b.transform.name == Templates.menuBody);
                PSystemBody planet = Utility.FindBody(PSystemManager.Instance.systemPrefab.rootBody, Templates.menuBody);
                if (planetCB == null || planet == null)
                {
                    planet = Utility.FindHomeBody(PSystemManager.Instance.systemPrefab.rootBody);
                    planetCB = PSystemManager.Instance.localBodies.Find(b => b.isHomeWorld);
                }
                if (planet == null || planetCB == null)
                {
                    Debug.LogError("[Kopernicus] Could not find homeworld!");
                    return;
                }

                // Load Textures
                OnDemand.OnDemandStorage.EnableBody(Templates.menuBody);

                // Get the MainMenu-Logic
                MainMenu main = FindObjectOfType<MainMenu>();
                if (main == null)
                {
                    Debug.LogError("[Kopernicus] No main menu object!");
                    return;
                }
                MainMenuEnvLogic logic = main.envLogic;

                // Set it to Space, because the Mun-Area won't work with sth else than Mun
                if (logic.areas.Length < 2)
                {
                    Debug.LogError("[Kopernicus] Not enough bodies");
                    return;
                }

                // Get our active Space
                GameObject space = logic.areas[1];

                // Deactivate Kerbins Transform
                Transform kerbin = space.transform.Find("Kerbin");
                if (kerbin == null)
                {
                    Debug.LogError("[Kopernicus] No Kerbin transform!");
                    return;
                }
                kerbin.gameObject.SetActive(false);

                // Deactivate Muns Transform
                Transform mun = space.transform.Find("MunPivot");
                if (mun == null)
                {
                    Debug.LogError("[Kopernicus] No MunPivot transform!");
                    return;
                }
                mun.gameObject.SetActive(false);

                // Activate the textures
                ScaledSpaceOnDemand od = planet.scaledVersion.GetComponentInChildren<ScaledSpaceOnDemand>();
                if (od != null)
                {
                    od.Start();
                    od.LoadTextures();
                }

                // Clone the scaledVersion and attach it to the Scene
                GameObject menuPlanet = Instantiate(planet.scaledVersion) as GameObject;
                menuPlanet.transform.parent = space.transform;

                // Destroy stuff
                DestroyImmediate(menuPlanet.GetComponent<ScaledSpaceFader>());
                DestroyImmediate(menuPlanet.GetComponent<SphereCollider>());
                DestroyImmediate(menuPlanet.GetComponentInChildren<AtmosphereFromGround>());
                DestroyImmediate(menuPlanet.GetComponent<MaterialSetDirection>());

                // That sounds funny
                Rotato planetRotato = menuPlanet.AddComponent<Rotato>();
                Rotato planetRefRotato = kerbin.GetComponent<Rotato>();
                planetRotato.speed = (planetRefRotato.speed / 9284.50070356553f) * (Single)planetCB.orbitDriver.orbit.orbitalSpeed; // calc.exe for the win

                // Scale the body
                menuPlanet.transform.localScale = kerbin.localScale;
                menuPlanet.transform.localPosition = kerbin.localPosition;
                menuPlanet.transform.position = kerbin.position;

                // And set it to layer 0
                menuPlanet.layer = 0;

                // Patch the material, because Mods like TextureReplacer run post spawn, and we'd overwrite their changes
                menuPlanet.GetComponent<Renderer>().sharedMaterial = planetCB.scaledBody.GetComponent<Renderer>().sharedMaterial;

                // Copy EVE 7.4 clouds / Rings
                for (Int32 i = 0; i < planetCB.scaledBody.transform.childCount; i++)
                {
                    // Just clone everything
                    Transform t = planetCB.scaledBody.transform.GetChild(i);
                    if (t.gameObject.GetComponent<AtmosphereFromGround>())
                        continue;
                    GameObject newT = Instantiate(t.gameObject) as GameObject;
                    newT.transform.parent = menuPlanet.transform;
                    newT.layer = 0;
                    newT.transform.localPosition = Vector3.zero;
                    newT.transform.localRotation = Quaternion.identity;
                    newT.transform.localScale = (Single)(1008 / planetCB.Radius) * Vector3.one;
                }

                // And now, create the moons
                foreach (PSystemBody moon in planet.children)
                {
                    // Grab the CeletialBody of the moon
                    CelestialBody moonCB = PSystemManager.Instance.localBodies.Find(b => b.GetTransform().name == moon.name);

                    // Create the Rotation-Transforms
                    GameObject menuMoonPivot = new GameObject(moon.name + " Pivot");
                    menuMoonPivot.gameObject.layer = 0;
                    menuMoonPivot.transform.position = menuPlanet.transform.position;

                    // Still funny...
                    Rotato munRotato = menuMoonPivot.AddComponent<Rotato>();
                    Rotato refRotato = mun.GetComponent<Rotato>();
                    munRotato.speed = (refRotato.speed / 542.494239600754f) * (Single)moonCB.GetOrbit().getOrbitalSpeedAtDistance(moonCB.GetOrbit().semiMajorAxis);

                    // Activate the textures
                    ScaledSpaceOnDemand odMoon = moon.scaledVersion.GetComponentInChildren<ScaledSpaceOnDemand>();
                    if (odMoon != null)
                    {
                        odMoon.Start();
                        odMoon.LoadTextures();
                    }

                    // Clone the scaledVersion and attach it to the pivot
                    GameObject menuMoon = Instantiate(moon.scaledVersion) as GameObject;
                    menuMoon.transform.parent = menuMoonPivot.transform;

                    // Move and scale the menuMoon correctly
                    menuMoon.transform.localPosition = new Vector3(-5000f * (Single)(moonCB.GetOrbit().semiMajorAxis / 12000000.0), 0f, 0f);
                    menuMoon.transform.localScale *= 7f;

                    // Destroy stuff
                    DestroyImmediate(menuMoon.GetComponent<ScaledSpaceFader>());
                    DestroyImmediate(menuMoon.GetComponent<SphereCollider>());
                    DestroyImmediate(menuMoon.GetComponentInChildren<AtmosphereFromGround>());
                    DestroyImmediate(menuMoon.GetComponent<MaterialSetDirection>());

                    // More Rotato
                    Rotato moonRotato = menuMoon.AddComponent<Rotato>();
                    moonRotato.speed = -0.005f / (Single)(moonCB.rotationPeriod / 400.0);

                    // Apply orbital stuff
                    menuMoon.transform.Rotate(0f, (Single)moonCB.orbitDriver.orbit.LAN, 0f);
                    menuMoon.transform.Rotate(0f, 0f, (Single)moonCB.orbitDriver.orbit.inclination);
                    menuMoon.transform.Rotate(0f, (Single)moonCB.orbitDriver.orbit.argumentOfPeriapsis, 0f);

                    // And set the layer to 0
                    menuMoon.layer = 0;

                    // Patch the material, because Mods like TextureReplacer run post spawn, and we'd overwrite their changes
                    menuMoon.GetComponent<Renderer>().sharedMaterial = moonCB.scaledBody.GetComponent<Renderer>().sharedMaterial;

                    // Copy EVE 7.4 clouds / Rings
                    for (Int32 i = 0; i < moonCB.scaledBody.transform.childCount; i++)
                    {
                        Transform t = moonCB.scaledBody.transform.GetChild(i);
                        if (t.gameObject.GetComponent<AtmosphereFromGround>())
                            continue;
                        GameObject newT = Instantiate(t.gameObject) as GameObject;
                        newT.transform.parent = menuMoon.transform;
                        newT.layer = 0;
                        newT.transform.localPosition = Vector3.zero;
                        newT.transform.localRotation = Quaternion.identity;
                        newT.transform.localScale = (Single)(1008 / moonCB.Radius) * Vector3.one;
                    }
                }
                Events.OnRuntimeUtilityUpdateMenu.Fire();
            }
        }
    }
}
