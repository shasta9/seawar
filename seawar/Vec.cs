using System;

namespace seawar {
   public class Vec : IEquatable<Vec> {
      public static Vec Zero => new Vec(0, 0);
      public static Vec Unit => new Vec(1, 1);

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

      public override string ToString() {
         return $"({X},{Y})";
      }

      public override bool Equals(object obj) {
         if (ReferenceEquals(null, obj)) return false;
         return obj is Vec vec && Equals(vec);
      }

      public bool Equals(Vec other) {
         return X == other.X && Y == other.Y;
      }

      public override int GetHashCode() {
         unchecked {
            return (X * 397) ^ Y;
         }
      }

      public static Vec operator +(Vec a, Vec b) {
         return new Vec(a.X + b.X, a.Y + b.Y);
      }

      public static Vec operator -(Vec a, Vec b) {
         return new Vec(a.X - b.X, a.Y - b.Y);
      }

      public static Vec operator *(Vec a, int multiplier) {
         return new Vec(a.X * multiplier, a.Y * multiplier);
      }

      public static bool operator ==(Vec a, Vec b) {
         return a.Equals(b);
      }

      public static bool operator !=(Vec a, Vec b) {
         return !(a == b);
      }
   }
}
