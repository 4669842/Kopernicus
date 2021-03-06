﻿/**
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

using CommNet;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Kopernicus
{
    namespace Configuration
    {
        namespace ModLoader
        {
            [RequireConfigType(ConfigType.Node)]
            public class City : ModLoader<PQSCity>, IParserEventSubscriber
            {
                // debugOrientated
                [ParserTarget("debugOrientated")]
                public NumericParser<Boolean> debugOrientated
                {
                    get { return mod.debugOrientated; }
                    set { mod.debugOrientated = value; }
                }

                // frameDelta
                [ParserTarget("frameDelta")]
                public NumericParser<Single> frameDelta
                {
                    get { return mod.frameDelta; }
                    set { mod.frameDelta = value; }
                }

                // randomizeOnSphere
                [ParserTarget("randomizeOnSphere")]
                public NumericParser<Boolean> randomizeOnSphere
                {
                    get { return mod.randomizeOnSphere; }
                    set { mod.randomizeOnSphere = value; }
                }

                // reorientToSphere
                [ParserTarget("reorientToSphere")]
                public NumericParser<Boolean> reorientToSphere
                {
                    get { return mod.reorientToSphere; }
                    set { mod.reorientToSphere = value; }
                }

                // reorientFinalAngle
                [ParserTarget("reorientFinalAngle")]
                public NumericParser<Single> reorientFinalAngle
                {
                    get { return mod.reorientFinalAngle; }
                    set { mod.reorientFinalAngle = value; }
                }

                // reorientInitialUp
                [ParserTarget("reorientInitialUp")]
                public Vector3Parser reorientInitialUp
                {
                    get { return mod.reorientInitialUp; }
                    set { mod.reorientInitialUp = value; }
                }

                // repositionRadial
                [ParserTarget("repositionRadial")]
                public Vector3Parser repositionRadial
                {
                    get { return mod.repositionRadial; }
                    set { mod.repositionRadial = value; }
                }

                // repositionRadiusOffset
                [ParserTarget("repositionRadiusOffset")]
                public NumericParser<Double> repositionRadiusOffset
                {
                    get { return mod.repositionRadiusOffset; }
                    set { mod.repositionRadiusOffset = value; }
                }

                // repositionToSphere
                [ParserTarget("repositionToSphere")]
                public NumericParser<Boolean> repositionToSphere
                {
                    get { return mod.repositionToSphere; }
                    set { mod.repositionToSphere = value; }
                }

                // repositionToSphereSurface
                [ParserTarget("repositionToSphereSurface")]
                public NumericParser<Boolean> repositionToSphereSurface
                {
                    get { return mod.repositionToSphereSurface; }
                    set { mod.repositionToSphereSurface = value; }
                }

                // repositionToSphereSurfaceAddHeight
                [ParserTarget("repositionToSphereSurfaceAddHeight")]
                public NumericParser<Boolean> repositionToSphereSurfaceAddHeight
                {
                    get { return mod.repositionToSphereSurfaceAddHeight; }
                    set { mod.repositionToSphereSurfaceAddHeight = value; }
                }

                // The mesh for the mod
                [ParserTarget("model")]
                public MuParser model
                {
                    set
                    {
                        value.Value.transform.parent = mod.transform;
                        Transform[] gameObjectList = mod.gameObject.GetComponentsInChildren<Transform>();
                        List<GameObject> rendererList = gameObjectList.Where(t => t.gameObject.GetComponent<Renderer>() != null).Select(t => t.gameObject).ToList();
                        mod.lod[0].objects = new GameObject[0];
                        mod.lod[0].renderers = rendererList.ToArray();
                    }
                }

                // visibility Range
                [ParserTarget("visibilityRange")]
                public NumericParser<Single> visibilityRange
                {
                    get { return mod.lod[0].visibleRange; }
                    set { mod.lod[0].visibleRange = value; }
                }

                // Commnet Station
                [ParserTarget("commnetStation")]
                public NumericParser<Boolean> commnetStation
                {
                    get { return mod.gameObject.GetComponentInChildren<CommNetHome>() != null; }
                    set
                    {
                        if (value)
                        {
                            CommNetHome station = mod.gameObject.AddComponent<CommNetHome>();
                            station.isKSC = false;
                        }
                    }
                }

                // Apply event
                void IParserEventSubscriber.Apply(ConfigNode node)
                {
                    mod.lod = new [] { new PQSCity.LODRange() };
                }

                // Apply event
                void IParserEventSubscriber.PostApply(ConfigNode node) { }
            }
        }
    }
}

