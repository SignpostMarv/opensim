/*
 * Copyright (c) Contributors, http://opensimulator.org/
 * See CONTRIBUTORS.TXT for a full list of copyright holders.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *     * Redistributions of source code must retain the above copyright
 *       notice, this list of conditions and the following disclaimer.
 *     * Redistributions in binary form must reproduce the above copyright
 *       notice, this list of conditions and the following disclaimer in the
 *       documentation and/or other materials provided with the distribution.
 *     * Neither the name of the OpenSimulator Project nor the
 *       names of its contributors may be used to endorse or promote products
 *       derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE DEVELOPERS ``AS IS'' AND ANY
 * EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL THE CONTRIBUTORS BE LIABLE FOR ANY
 * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */
using System;
using OpenSim.Framework;
using OpenSim.Region.Framework.Interfaces;
using OpenSim.Region.Framework.Scenes;

namespace OpenSim.Region.CoreModules.World.Terrain.Effects
{
    /// <summary>
    /// Uses perlin noise based on grid coordinates
    /// </summary>
    public class GridPerlin : ITerrainRegionInfoEffect
    {
        public void RunEffect(ITerrainChannel map)
        {
            doEffect(map, 0, 0);
        }

        public void RunEffect(ITerrainChannel map, RegionInfo info)
        {
            doEffect(map, info.RegionLocX, info.RegionLocY);
        }

        private void doEffect(ITerrainChannel map, uint seedX, uint seedY)
        {
            for (int x = 0; x < map.Width; ++x)
            {
                double xPart = seedX + ((double)x / (double)Math.Max(1, map.Width));
                for (int y = 0; y < map.Height; ++y)
                {
                    double yPart = seedY + ((double)y / (double)Math.Max(1, map.Height));
                    double noiseA = TerrainUtil.PerlinNoise2D(xPart, yPart, 3, 2.0) * 8;
                    double noiseB = TerrainUtil.PerlinNoise2D(xPart, yPart, 4, 2.0) * 4;
                    map[x, y] = 32 + noiseA + noiseB;
                }
            }

            TerrainEffectUtil.SmoothMap(map, 3, 4.0);
        }
    }
}
