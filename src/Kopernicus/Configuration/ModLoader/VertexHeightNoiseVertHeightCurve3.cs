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

using LibNoise;
using System;
using UnityEngine;

namespace Kopernicus
{
    namespace Configuration
    {
        namespace ModLoader
        {
            [RequireConfigType(ConfigType.Node)]
            public class VertexHeightNoiseVertHeightCurve3 : ModLoader<PQSMod_VertexHeightNoiseVertHeightCurve3>
            {
                // Maximum deformity
                [ParserTarget("deformityMax")]
                public NumericParser<Double> deformityMax
                {
                    get { return mod.deformityMax; }
                    set { mod.deformityMax = value; }
                }

                // Minimum deformity
                [ParserTarget("deformityMin")]
                public NumericParser<Double> deformityMin
                {
                    get { return mod.deformityMin; }
                    set { mod.deformityMin = value; }
                }

                // Deformity multiplier curve
                [ParserTarget("inputHeightCurve")]
                public FloatCurveParser inputHeightCurve
                {
                    get { return mod.inputHeightCurve; }
                    set { mod.inputHeightCurve = value; }
                }

                // Ending height
                [ParserTarget("inputHeightEnd")]
                public NumericParser<Double> inputHeightEnd
                {
                    get { return mod.inputHeightEnd; }
                    set { mod.inputHeightEnd = value; }
                }

                // Starting height
                [ParserTarget("inputHeightStart")]
                public NumericParser<Double> inputHeightStart
                {
                    get { return mod.inputHeightStart; }
                    set { mod.inputHeightStart = value; }
                }

                // The frequency of the simplex multiplier
                [ParserTarget("multiplierFrequency")]
                public NumericParser<Double> multiplierFrequency
                {
                    get { return mod.curveMultiplier.frequency; }
                    set { mod.curveMultiplier.frequency = value; }
                }

                // Octaves of the simplex multiplier
                [ParserTarget("multiplierOctaves")]
                public NumericParser<Int32> multiplierOctaves
                {
                    get { return mod.curveMultiplier.octaves; }
                    set { mod.curveMultiplier.octaves = value; }
                }

                // Persistence of the simplex multiplier
                [ParserTarget("multiplierPersistence")]
                public NumericParser<Double> multiplierPersistence
                {
                    get { return mod.curveMultiplier.persistence; }
                    set { mod.curveMultiplier.persistence = value; }
                }

                // The seed of the simplex multiplier
                [ParserTarget("multiplierSeed")]
                public NumericParser<Int32> multiplierSeed
                {
                    get { return mod.curveMultiplier.seed; }
                    set { mod.curveMultiplier.seed = value; }
                }

                // The frequency of the simplex noise on deformity
                [ParserTarget("deformityFrequency")]
                public NumericParser<Double> deformityFrequency
                {
                    get { return mod.deformity.frequency; }
                    set { mod.deformity.frequency = value; }
                }

                // Octaves of the simplex noise on deformity
                [ParserTarget("deformityOctaves")]
                public NumericParser<Int32> deformityOctaves
                {
                    get { return mod.deformity.octaves; }
                    set { mod.deformity.octaves = value; }
                }

                // Persistence of the simplex noise on deformity
                [ParserTarget("deformityPersistence")]
                public NumericParser<Double> deformityPersistence
                {
                    get { return mod.deformity.persistence; }
                    set { mod.deformity.persistence = value; }
                }

                // The seed of the simplex noise on deformity
                [ParserTarget("deformitySeed")]
                public NumericParser<Int32> deformitySeed
                {
                    get { return mod.deformity.seed; }
                    set { mod.deformity.seed = value; }
                }

                // The frequency of the additive noise
                [ParserTarget("ridgedAddFrequency")]
                public NumericParser<Double> ridgedAddFrequency
                {
                    get { return mod.ridgedAdd.frequency; }
                    set { mod.ridgedAdd.frequency = value; }
                }

                // Lacunarity of the additive noise
                [ParserTarget("ridgedAddLacunarity")]
                public NumericParser<Double> ridgedAddLacunarity
                {
                    get { return mod.ridgedAdd.lacunarity; }
                    set { mod.ridgedAdd.lacunarity = value; }
                }

                // Octaves of the additive noise
                [ParserTarget("ridgedAddOctaves")]
                public NumericParser<Int32> ridgedAddOctaves
                {
                    get { return mod.ridgedAdd.octaves; }
                    set { mod.ridgedAdd.octaves = Mathf.Clamp(value, 1, 30); }
                }

                // The quality of the additive noise
                [ParserTarget("ridgedAddQuality")]
                public EnumParser<KopernicusNoiseQuality> ridgedAddQuality
                {
                    get { return (KopernicusNoiseQuality) (Int32) mod.ridgedAdd.quality; }
                    set { mod.ridgedAdd.quality = (NoiseQuality) (Int32) value.Value; }
                }

                // The seed of the additive noise
                [ParserTarget("ridgedAddSeed")]
                public NumericParser<Int32> ridgedAddSeed
                {
                    get { return mod.ridgedAdd.seed; }
                    set { mod.ridgedAdd.seed = value; }
                }

                // The frequency of the subtractive noise
                [ParserTarget("ridgedSubFrequency")]
                public NumericParser<Double> ridgedSubFrequency
                {
                    get { return mod.ridgedSub.frequency; }
                    set { mod.ridgedSub.frequency = value; }
                }

                // Lacunarity of the subtractive noise
                [ParserTarget("ridgedSubLacunarity")]
                public NumericParser<Double> ridgedSubLacunarity
                {
                    get { return mod.ridgedSub.lacunarity; }
                    set { mod.ridgedSub.lacunarity = value; }
                }

                // Octaves of the subtractive noise
                [ParserTarget("ridgedSubOctaves")]
                public NumericParser<Int32> ridgedSubOctaves
                {
                    get { return mod.ridgedSub.octaves; }
                    set { mod.ridgedSub.octaves = Mathf.Clamp(value, 1, 30); }
                }

                // The quality of the subtractive noise
                [ParserTarget("ridgedSubQuality")]
                public EnumParser<KopernicusNoiseQuality> ridgedSubQuality
                {
                    get { return (KopernicusNoiseQuality) (Int32) mod.ridgedSub.quality; }
                    set { mod.ridgedSub.quality = (NoiseQuality) (Int32) value.Value; }
                }

                // The seed of the subtractive noise
                [ParserTarget("ridgedSubSeed")]
                public NumericParser<Int32> ridgedSubSeed
                {
                    get { return mod.ridgedSub.seed; }
                    set { mod.ridgedSub.seed = value; }
                }

                // Create the mod
                public override void Create(PQS pqsVersion)
                {
                    base.Create(pqsVersion);

                    // Construct the internal objects.
                    mod.curveMultiplier = new PQSMod_VertexHeightNoiseVertHeightCurve3.SimplexNoise();
                    mod.deformity = new PQSMod_VertexHeightNoiseVertHeightCurve3.SimplexNoise();
                    mod.ridgedAdd = new PQSMod_VertexHeightNoiseVertHeightCurve3.RidgedNoise();
                    mod.ridgedSub = new PQSMod_VertexHeightNoiseVertHeightCurve3.RidgedNoise();
                }

                // Create the mod
                public override void Create(PQSMod_VertexHeightNoiseVertHeightCurve3 _mod, PQS pqsVersion)
                {
                    base.Create(_mod, pqsVersion);

                    // Construct the internal objects.
                    if (mod.curveMultiplier == null)
                    {
                        mod.curveMultiplier = new PQSMod_VertexHeightNoiseVertHeightCurve3.SimplexNoise();
                    }
                    if (mod.deformity == null)
                    {
                        mod.deformity = new PQSMod_VertexHeightNoiseVertHeightCurve3.SimplexNoise();
                    }
                    if (mod.ridgedAdd == null)
                    {
                        mod.ridgedAdd = new PQSMod_VertexHeightNoiseVertHeightCurve3.RidgedNoise();
                    }
                    if (mod.ridgedSub == null)
                    {
                        mod.ridgedSub = new PQSMod_VertexHeightNoiseVertHeightCurve3.RidgedNoise();
                    }
                }
            }
        }
    }
}


