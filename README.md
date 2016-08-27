# UnityIntegerPoint
A fairly comprehensive integer Point struct, in 2D and 3D. Basically a vector, but integer.

While building some puzzle games in Unity, I found myself missing a real integer Point struct. While Vector2 and Vector3 are great, I found myself doing so much casting and rounding that I decided there needed something simpler.

There are two structs, Point2 and Point3. They should interact nicely with the existing Unity Vector2 and Vector3.

There's also some handy helper functions if you need a Vector rounded rather than just truncated (by cast). Or if you want to get items from a 2D or 3D collection with a point.

The direction enum is so you can do stuff like this easily. . .

```
// look around in 8 directions
for (var i = Direction.First; i < Direction.Length; ++i)
   LookInDirection(new Point2(i));
```

The classes are lightweight (two or three integers), and most of the functions are one-liners, so this should optimize nicely. Lots of operator overloading, so you can do readable stuff like this. . .

```
// move the piece five spaces north
PieceLoc += Point2.north * 5;
```

Also note that this should be very easily modifiable for other C# environments. The only Unity-dependent code are the casts and functions to deal with Vector2 and Vector3. Delete those, and you should be good to go.
