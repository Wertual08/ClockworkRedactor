using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExtraForms;
using ExtraForms.OpenGL;

namespace ResrouceRedactor.Resources.Redactors
{
    class PreviewSurface : OpenGLSurface
    {
        protected override void OnMouseDown(MouseEventArgs e)
        {
            Focus();
            base.OnMouseDown(e);
        }

        public class Entity
        {
            public double PositionX = 0d, PositionY = 0d;
            public double VelocityX = 0d, VelocityY = 0d;
            public double AccelerationX = 0d, AccelerationY = 0d;
            public bool OnGround = false;
            public bool OnRoof = false;
            public bool OnRWall = false;
            public bool OnLWall = false;
            public int Direction = 1;
            public bool Disposable { get; private set; }
            public EntityResource Resource;

            public Subresource<ToolResource> Tool = new Subresource<ToolResource>();
            public bool ToolEnabled = true;

            public Entity(string path)
            {
                Resource = new EntityResource(path);
                Disposable = true;
            }
            public Entity(EntityResource resource)
            {
                Resource = resource;
                Disposable = false;
            }

            public int ToolCycle = 0;
            public double CursX = 0d;
            public double CursY = 0d;
            public bool Fire = false;
            public bool MoveU = false;
            public bool MoveL = false;
            public bool MoveD = false;
            public bool MoveR = false;
            public bool Jump = false;
        }
        public class Tile
        {
            public int PositionX;
            public int PositionY;
            public TileResource Resource;
            public TileResource[] Ambit = new TileResource[8];
            public Tile(int x, int y, TileResource r)
            {
                PositionX = x;
                PositionY = y;
                Resource = r;
            }
        }
        public double FieldW = 32d, FieldH = 32d;
        private Dictionary<string, Entity> Entities = new Dictionary<string, Entity>();
        private List<Tile> Tiles = new List<Tile>();
        private Dictionary<string, TileResource> TileNames = new Dictionary<string, TileResource>();

        private void UpdateTileAmbit(int x, int y)
        {
            var tile = GetTile(x, y);
            if (tile != null)
            {
                tile.Ambit[0] = GetTile(x, y + 1)?.Resource;
                tile.Ambit[1] = GetTile(x - 1, y + 1)?.Resource;
                tile.Ambit[2] = GetTile(x - 1, y)?.Resource;
                tile.Ambit[3] = GetTile(x - 1, y - 1)?.Resource;
                tile.Ambit[4] = GetTile(x, y - 1)?.Resource;
                tile.Ambit[5] = GetTile(x + 1, y - 1)?.Resource;
                tile.Ambit[6] = GetTile(x + 1, y)?.Resource;
                tile.Ambit[7] = GetTile(x + 1, y + 1)?.Resource;
            }
        }
        private bool ValidateTrigger(Entity entity, EntityResource.Trigger trigger)
        {
            int DoNotCare = EntityResource.Trigger.DoNotCare;
            return 
                Math.Abs(entity.VelocityX) >= trigger.VelocityXLowBound &&
                Math.Abs(entity.VelocityX) <= trigger.VelocityXHighBound &&
                Math.Abs(entity.VelocityY) >= trigger.VelocityYLowBound &&
                Math.Abs(entity.VelocityY) <= trigger.VelocityYHighBound &&
                Math.Abs(entity.AccelerationX) >= trigger.AccelerationXLowBound &&
                Math.Abs(entity.AccelerationX) <= trigger.AccelerationXHighBound &&
                Math.Abs(entity.AccelerationY) >= trigger.AccelerationYLowBound &&
                Math.Abs(entity.AccelerationY) <= trigger.AccelerationYHighBound &&
                (trigger.OnGround == DoNotCare || entity.OnGround ==
                (trigger.IOnGround == EntityResource.Trigger.BoolConditional.True)) &&
                (trigger.OnRoof == DoNotCare || entity.OnRoof ==
                (trigger.IOnRoof == EntityResource.Trigger.BoolConditional.True)) &&
                (trigger.OnWall == DoNotCare || 
                (entity.OnRWall && trigger.IOnWall == EntityResource.Trigger.DirectionConditional.Right) ||
                (entity.OnLWall && trigger.IOnWall == EntityResource.Trigger.DirectionConditional.Left) ||
                ((entity.OnRWall || entity.OnLWall) && trigger.IOnWall == EntityResource.Trigger.DirectionConditional.Both)) &&
                (trigger.Direction == DoNotCare || trigger.IDirection == EntityResource.Trigger.DirectionConditional.Both ||
                (entity.Direction == 1 && (trigger.IDirection == EntityResource.Trigger.DirectionConditional.Right)) || 
                (entity.Direction == -1 && (trigger.IDirection == EntityResource.Trigger.DirectionConditional.Left)));
        }

        public PreviewSurface()
        {
            
        }

        public void SetFieldSize(double w, double h)
        {
            FieldW = w;
            FieldH = h;
        }

        public bool GridEnabled = false;

        public void ResortTiles()
        {
            for (int i = 1; i < Tiles.Count - 1; i++)
            {
                for (int j = 0; j < Tiles.Count - i; j++)
                {
                    if (Tiles[j].Resource.Layer > Tiles[j + 1].Resource.Layer)
                    {
                        var t = Tiles[j];
                        Tiles[j] = Tiles[j + 1];
                        Tiles[j + 1] = t;
                    }
                }
            }

            //Tiles.Sort((Tile l, Tile r) =>
            //{ 
            //    return l.Resource.Layer > r.Resource.Layer ? 1 : 
            //        (l.Resource.Layer == r.Resource.Layer ? 0 : -1); 
            //});
        }
        public bool TileLoaded(string name)
        {
            return TileNames.ContainsKey(name);
        }
        public bool LoadTile(string path, string name)
        {
            try 
            {
                MakeCurrent();
                return LoadTile(new TileResource(path), name);
            }
            catch 
            {
                return false;
            }
        }
        public bool LoadTile(TileResource resource, string name)
        {
            try
            {
                if (TileNames.ContainsKey(name))
                {
                    var old = TileNames[name];
                    for (int i = 0; i < Tiles.Count; i++) if (Tiles[i].Resource == old) Tiles[i].Resource = resource;
                    TileNames[name] = resource;
                }
                else TileNames.Add(name, resource);
                ResortTiles();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool UnloadTile(string name)
        {
            try
            {
                MakeCurrent();
                var tile = TileNames[name];
                tile.Dispose();
                return TileNames.Remove(name);
            }
            catch
            {
                return false;
            }
        }
        public Tile GetTile(int x, int y)
        {
            return Tiles.Find((Tile t) => { return x == t.PositionX && y == t.PositionY; });
        }
        public void PlaceTile(string name, int x, int y)
        {
            if (x < 0 || x + 0.5 > FieldW || y < 0 || y + 0.5 > FieldH) return;
            if (name != null && TileNames.ContainsKey(name))
            {
                var resource = TileNames[name];
                var tile = GetTile(x, y);
                if (tile != null) Tiles.Remove(tile);
                Tiles.Insert(Tiles.FindLastIndex((Tile t) => { return t.Resource.Layer < resource.Layer; }) + 1, new Tile(x, y, resource));
                
                //ResortTiles();
            }
            else Tiles.RemoveAll((Tile t) => { return x == t.PositionX && y == t.PositionY; });
            for (int ox = -1; ox <= 1; ox++)
                for (int oy = -1; oy <= 1; oy++)
                    UpdateTileAmbit(x + ox, y + oy);
        }

        public bool LoadEntity(string path, string name)
        {
            try
            {
                MakeCurrent();
                Entities.Add(name, new Entity(path));
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool LoadEntity(EntityResource resource, string name)
        {
            try
            {
                MakeCurrent();
                Entities.Add(name, new Entity(resource));
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool UnloadEntity(string name)
        {
            try
            {
                MakeCurrent();
                if (Entities[name].Disposable) Entities[name].Resource?.Dispose();
                Entities[name].Tool?.Dispose();
                return Entities.Remove(name);
            }
            catch
            {
                return false;
            }
        }
        public Entity GetEntity(string name)
        {
            if (Entities.ContainsKey(name)) return Entities[name];
            else return null;
        }

        public double OffsetAngle(double vx, double vy, double dx, double dy, double px, double py)
        {
            return Math.Atan2(py, px);
        }

        public void Update(double dt)
        {
            foreach  (var e in Entities)
            {
                var entity = e.Value;
                var resource = entity.Resource;
                var ragdoll = resource.Ragdoll.Resource;
                var tool = entity.ToolEnabled ? entity.Tool.Resource : null;
                double rhw = ragdoll?.HitboxW / 2d ?? 0d;
                double rhh = ragdoll?.HitboxH / 2d ?? 0d;
                double fhw = FieldW / 2d;
                double fhh = FieldH / 2d;

                double move_fx = 0d;
                double move_fy = 0d;

                if (entity.MoveR) move_fx += resource.MoveForceX;
                if (entity.MoveL) move_fx -= resource.MoveForceX;
                if (entity.MoveU) move_fy += resource.MoveForceY;
                if (entity.MoveD) move_fy -= resource.MoveForceY;

                entity.AccelerationX = (move_fx - entity.VelocityX * (resource.DragX + 
                    Math.Abs(entity.VelocityX) * resource.SqrDragX)) / resource.Mass;
                entity.AccelerationY = (move_fy - entity.VelocityY * (resource.DragY + 
                    Math.Abs(entity.VelocityY) * resource.SqrDragY)) / resource.Mass;

                entity.AccelerationY += resource.GravityAcceleration;

                entity.VelocityX += entity.AccelerationX * dt;
                entity.VelocityY += entity.AccelerationY * dt;

                entity.PositionX += entity.VelocityX * dt;
                entity.PositionY += entity.VelocityY * dt;

                entity.OnGround = false;

                if (entity.PositionX + rhw >= fhw)
                {
                    entity.PositionX = fhw - rhw;
                    if (entity.VelocityX > 0d) entity.VelocityX = 0d;
                }
                if (entity.PositionX - rhw <= -fhw)
                {
                    entity.PositionX = -fhw + rhw;
                    if (entity.VelocityX < 0d) entity.VelocityX = 0d;
                }
                if (entity.PositionY + rhh >= fhh)
                {
                    entity.PositionY = fhh - rhh;
                    if (entity.VelocityY > 0d) entity.VelocityY = 0d;
                }
                if (entity.PositionY - rhh <= -fhh)
                {
                    entity.PositionY = -fhh + rhh;
                    if (entity.VelocityY < 0d) entity.VelocityY = 0d;
                    entity.OnGround = true;

                    if (entity.Jump) entity.VelocityY = resource.JumpVelocity;
                }

                if (entity.VelocityX > 0d) entity.Direction = 1;
                if (entity.VelocityX < 0d) entity.Direction = -1;
                if (tool != null)
                {
                    if (entity.CursX > entity.PositionX) entity.Direction = 1;
                    if (entity.CursX < entity.PositionX) entity.Direction = -1;
                }

                foreach (var t in resource.Triggers)
                {
                    var animation = t.Animation.Resource;
                    if (animation == null) continue;

                    if (!ValidateTrigger(entity, t)) animation.Stop();
                    else if (!animation.Playing) animation.Play();

                    if (animation.Playing)
                    {
                        double fpur = 0d;
                        switch (animation.Dependency)
                        {
                            case AnimationType.TimeLoop:
                            case AnimationType.TimeOnce:
                                {
                                }
                                break;

                            case AnimationType.MovementX:
                                {
                                    fpur = entity.VelocityX * entity.Direction;
                                }
                                break;

                            case AnimationType.MovementY:
                                {
                                    fpur = Math.Abs(entity.VelocityY);
                                }
                                break;

                            case AnimationType.VelocityX:
                                {
                                    fpur = entity.VelocityX;
                                }
                                break;

                            case AnimationType.VelocityY:
                                {
                                    fpur = entity.VelocityY;
                                }
                                break;

                            case AnimationType.AimCursor:
                                {
                                }
                                break;

                            case AnimationType.FollowCursor:
                                {
                                }
                                break;
                        }

                        animation.Update((float)dt, (float)fpur);
                    }
                }

                if (tool != null)
                {
                    EntityResource.Holder holder = null;
                    foreach (var h in resource.Holders)
                    {
                        var animation = h.Animation.Resource;
                        if (animation == null) continue;
                        if (h.Action == tool.ActionName)
                        {
                            holder = h;
                            break;
                        }
                    }
                    holder?.Animation.Resource?.Update((float)dt, (float)tool.Progress);

                    if (entity.Fire && tool.CheckKD(entity.ToolCycle))
                    {
                        tool.BeginCycle();
                        holder?.Animation.Resource?.Play();
                    }
                }
            }
        }
        public void Render(float scx, float scy)
        {
            if (GridEnabled)
            {
                gl.Disable(GL.TEXTURE_2D);
                gl.Begin(GL.LINES);
                var color = BackColor;
                gl.Color4ub((byte)((color.R + 128) % 256), (byte)((color.G + 128) % 256), (byte)((color.B + 128) % 256), 255);
                for (float x = -(float)FieldW / 2f + 1; x < (float)FieldW / 2f; x++)
                {
                    gl.Vertex2f(x - scx, -(float)FieldH / 2f - scy);
                    gl.Vertex2f(x - scx, (float)FieldH / 2f - scy);
                }
                for (float y = -(float)FieldH / 2f + 1; y < (float)FieldH / 2f; y++)
                {
                    gl.Vertex2f(-(float)FieldW / 2f - scx, y - scy);
                    gl.Vertex2f((float)FieldW / 2f - scx, y - scy);
                }
                gl.End();
            }

            long time = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            gl.Color4ub(0xff, 0xff, 0xff, 0xff);
            //byte a = 20;
            foreach (var t in Tiles)
            {
                //gl.Color4ub(0xff, 0xff, 0xff, a);
                //a += 20;
                t.Resource?.Render(-scx + t.PositionX - (float)FieldW / 2.0f + 0.5f, -scy + t.PositionY - (float)FieldH / 2.0f + 0.5f, time, t.Ambit);
            }
            foreach (var e in Entities)
            {

                var entity = e.Value;
                var resource = entity.Resource;
                var ragdoll = resource.Ragdoll.Resource;
                var tool = entity.ToolEnabled ? entity.Tool.Resource : null;
                if (ragdoll == null) continue;
                var frame = new Frame(ragdoll.Count);
                foreach (var t in resource.Triggers)
                {
                    var animation = t.Animation.Resource;
                    if (animation == null) continue;
                    if (animation.Playing) frame.Overlap(animation.CurrentFrame);
                }

                float sx = 1f, sy = 1f, sa = 1f;
                sx *= entity.Direction;
                sa *= entity.Direction;

                gl.Disable(GL.TEXTURE_2D);
                gl.Begin(GL.LINES);
                gl.Color4ub(0, 0, 255, 255);
                gl.Vertex2f(-scx + (float)ragdoll.HitboxW / 2f + (float)entity.PositionX, -scy + (float)ragdoll.HitboxH / 2f + (float)entity.PositionY);
                gl.Vertex2f(-scx - (float)ragdoll.HitboxW / 2f + (float)entity.PositionX, -scy + (float)ragdoll.HitboxH / 2f + (float)entity.PositionY);
                gl.Vertex2f(-scx - (float)ragdoll.HitboxW / 2f + (float)entity.PositionX, -scy + (float)ragdoll.HitboxH / 2f + (float)entity.PositionY);
                gl.Vertex2f(-scx - (float)ragdoll.HitboxW / 2f + (float)entity.PositionX, -scy - (float)ragdoll.HitboxH / 2f + (float)entity.PositionY);
                gl.Vertex2f(-scx - (float)ragdoll.HitboxW / 2f + (float)entity.PositionX, -scy - (float)ragdoll.HitboxH / 2f + (float)entity.PositionY);
                gl.Vertex2f(-scx + (float)ragdoll.HitboxW / 2f + (float)entity.PositionX, -scy - (float)ragdoll.HitboxH / 2f + (float)entity.PositionY);
                gl.Vertex2f(-scx + (float)ragdoll.HitboxW / 2f + (float)entity.PositionX, -scy - (float)ragdoll.HitboxH / 2f + (float)entity.PositionY);
                gl.Vertex2f(-scx + (float)ragdoll.HitboxW / 2f + (float)entity.PositionX, -scy + (float)ragdoll.HitboxH / 2f + (float)entity.PositionY);
                gl.End();

                gl.Color4ub(255, 255, 255, 255);
                if (tool != null)
                {
                    int hp = -1;
                    foreach (var h in resource.Holders)
                    {
                        var animation = h.Animation.Resource;
                        if (animation == null) continue;
                        if (h.Action == tool.ActionName)
                        {
                            if (h.HolderPoint < 0 || h.HolderPoint >= ragdoll.Count) continue;
                            hp = h.HolderPoint;
                            int p = hp;
                            var f = animation.CurrentFrame;
                            while (ragdoll[p].MainNode >= 0 && f[ragdoll[p].MainNode][NodeProperties.Enabled]) p = ragdoll[p].MainNode;

                            var tf = ragdoll.MakeFrame(Frame.Overlap(frame, f), sx, sy, sa);

                            double sn = Math.Sin(tf[hp].Angle);
                            double cs = Math.Cos(tf[hp].Angle);
                            double vx = tf[hp].OffsetX - tf[p].OffsetX + tool.FirePointX * cs - tool.FirePointY * sn;
                            double vy = tf[hp].OffsetY - tf[p].OffsetY + tool.FirePointX * sn + tool.FirePointY * cs;
                            double dx = tool.FireVectorX;
                            double dy = tool.FireVectorY;
                            double px = entity.CursX - tf[p].OffsetX - entity.PositionX;
                            double py = entity.CursY - tf[p].OffsetY - entity.PositionY;
                            frame.Overlap(f.Rotate((float)OffsetAngle(vx, vy, dx, dy, px * sx, py * sy)));

                            break;
                        }
                    }
                    ragdoll.Render(ragdoll.MakeFrame(frame, sx, sy, sa), (float)e.Value.PositionX - scx, 
                        (float)e.Value.PositionY - scy, time, sx, sy, 1f, tool, hp, entity.ToolCycle);
                }
                else
                {
                    ragdoll.Render(ragdoll.MakeFrame(frame, sx, sy, sa),
                        (float)e.Value.PositionX - scx, (float)e.Value.PositionY - scy, time, sx, sy);
                }
            }

            gl.Disable(GL.TEXTURE_2D);
            gl.Begin(GL.LINES);
            gl.Color4ub(0, 255, 255, 255);
            gl.Vertex2f(-scx + (float)FieldW / 2f, -scy + (float)FieldH / 2f);
            gl.Vertex2f(-scx - (float)FieldW / 2f, -scy + (float)FieldH / 2f);
            gl.Vertex2f(-scx - (float)FieldW / 2f, -scy + (float)FieldH / 2f);
            gl.Vertex2f(-scx - (float)FieldW / 2f, -scy - (float)FieldH / 2f);
            gl.Vertex2f(-scx - (float)FieldW / 2f, -scy - (float)FieldH / 2f);
            gl.Vertex2f(-scx + (float)FieldW / 2f, -scy - (float)FieldH / 2f);
            gl.Vertex2f(-scx + (float)FieldW / 2f, -scy - (float)FieldH / 2f);
            gl.Vertex2f(-scx + (float)FieldW / 2f, -scy + (float)FieldH / 2f);
            gl.End();
        }

        protected override void Dispose(bool disposing)
        {
            if (IsDisposed) return;

            if (disposing)
            {
                MakeCurrent();
                foreach (var tile in TileNames) tile.Value?.Dispose();
                foreach (var entity in Entities)
                {
                    if (entity.Value.Disposable) entity.Value.Resource?.Dispose();
                    entity.Value.Tool?.Dispose();
                }
            }

            base.Dispose(disposing);
        }
    }
}
