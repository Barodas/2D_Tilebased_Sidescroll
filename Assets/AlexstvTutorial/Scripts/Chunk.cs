﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ASTutorial
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]
    public class Chunk : MonoBehaviour
    {
        private Block[,,] _blocks = new Block[chunkSize, chunkSize, chunkSize];
        private MeshFilter _filter;
        private MeshCollider _col;

        public static int chunkSize = 16;
        public bool update = true;
        public World world;
        public WorldPos pos;

        private void Start()
        {
            _filter = GetComponent<MeshFilter>();
            _col = GetComponent<MeshCollider>();

            //// Example Chunk Code
            //_blocks = new Block[chunkSize, chunkSize, chunkSize];
            //for(int x = 0; x < chunkSize; x++)
            //{
            //    for(int y = 0; y < chunkSize; y++)
            //    {
            //        for(int z = 0; z < chunkSize; z++)
            //        {
            //            _blocks[x, y, z] = new BlockAir();
            //        }
            //    }
            //}
            //_blocks[3, 5, 2] = new Block();
            //_blocks[4,5,2] = new BlockGrass();
            //UpdateChunk();
            //// End Example Chunk Code
        }

        private void Update()
        {
            if(update)
            {
                update = false;
                UpdateChunk();
                Debug.Log("UpdatingChunk");
            }
        }

        public void SetBlock(int x, int y, int z, Block block)
        {
            if (InRange(x) && InRange(y) && InRange(z))
            {
                _blocks[x, y, z] = block;
            }
            else
            {
                world.SetBlock(pos.x + x, pos.y + y, pos.z + z, block);
            }
        }

        public Block GetBlock(int x, int y, int z)
        {
            if (InRange(x) && InRange(y) && InRange(z))
            {
                return _blocks[x, y, z];
            }
            return world.GetBlock(pos.x + x, pos.y + y, pos.z + z);
        }

        public static bool InRange(int index)
        {
            if (index < 0 || index >= chunkSize)
            {
                return false;
            }
            return true;
        }

        private void UpdateChunk()
        {
            MeshData meshData = new MeshData();

            for (int x = 0; x < chunkSize; x++)
            {
                for (int y = 0; y < chunkSize; y++)
                {
                    for (int z = 0; z < chunkSize; z++)
                    {
                        meshData = _blocks[x, y, z].BlockData(this, x, y, z, meshData);
                    }
                }
            }
            RenderMesh(meshData);
        }

        private void RenderMesh(MeshData meshData)
        {
            _filter.mesh.Clear();
            _filter.mesh.vertices = meshData.vertices.ToArray();
            _filter.mesh.triangles = meshData.triangles.ToArray();
            _filter.mesh.uv = meshData.uv.ToArray();
            _filter.mesh.RecalculateNormals();

            _col.sharedMesh = null;
            Mesh mesh = new Mesh();
            mesh.vertices = meshData.colVertices.ToArray();
            mesh.triangles = meshData.colTriangles.ToArray();
            mesh.RecalculateNormals();
            _col.sharedMesh = mesh;
        }
    }
}