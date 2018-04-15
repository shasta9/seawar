using System;

namespace seawar
{
   public struct Vec {
      public int X { get; }
      public int Y { get; }

      public double Bearing { get; }
      public double Length { get; }

      public Vec(int x, int y) {
         X = x;
         Y = y;
         if (X == 0) {
            Length = Math.Abs(Y);
            Bearing = (Y >= 0) ? 0.0 : 180.0;
         }
         else {
            Length = Math.Sqrt(X * X + Y * Y);
            Bearing = Math.Atan2(X, Y) * 360.0 / (2.0 * Math.PI);
            if (Bearing < 0.0) Bearing = Bearing + 360.0;
         }
      }

      public override bool Equals(object obj) {
         return base.Equals(obj);
      }

      public bool Equals(Vec other) {
         return X == other.X && Y == other.Y;
      }

      public override int GetHashCode() {
         unchecked {
            return (X * 397) ^ Y;
         }
      }

      public override string ToString() {
         return $"({X},{Y})";
      }

      public static Vec operator +(Vec a, Vec b) {
         return new Vec(a.X + b.X, a.Y + b.Y);
      }

      public static Vec operator -(Vec a, Vec b) {
         return new Vec(a.X - b.X, a.Y - b.Y);
      }
   }
}