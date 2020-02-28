// Given a 3d position as a seed, compute an even smoother procedural noise
// value. "Improving Noise" [Perlin 2002].
//
// Inputs:
//   st  3D seed
// Values between  -½ and ½ ?
//
// expects: random_direction, improved_smooth_step
float improved_perlin_noise( vec3 st) 
{
  /////////////////////////////////////////////////////////////////////////////
  // Replace with your code 
  // Reference: https://en.wikipedia.org/wiki/Perlin_noise
  // Determine grid cell coordinates
  int x0 = int(floor(st.x));
  int y0 = int(floor(st.y));
  int z0 = int(floor(st.z));

  int x1 = x0 + 1;
  int y1 = y0 + 1;
  int z1 = z0 + 1;

  float sx = st.x - float(x0);
  float sy = st.y - float(y0);
  float sz = st.z - float(z0);

  vec3 c1 = vec3(x0, y0, z0);
  vec3 c2 = vec3(x1, y0, z0);
  vec3 c3 = vec3(x0, y1, z0);
  vec3 c4 = vec3(x1, y1, z0);
  vec3 c5 = vec3(x0, y0, z1);
  vec3 c6 = vec3(x1, y0, z1);
  vec3 c7 = vec3(x0, y1, z1);
  vec3 c8 = vec3(x1, y1, z1);

  //random gradient:
  vec3 g1 = random_direction(c1);
  vec3 g2 = random_direction(c2);
  vec3 g3 = random_direction(c3);
  vec3 g4 = random_direction(c4);
  vec3 g5 = random_direction(c5);
  vec3 g6 = random_direction(c6);
  vec3 g7 = random_direction(c7);
  vec3 g8 = random_direction(c8);

  //difference betweeen seed and corner: st - corner:
  vec3 d1 = st - c1;
  vec3 d2 = st - c2;
  vec3 d3 = st - c3;
  vec3 d4 = st - c4;
  vec3 d5 = st - c5;
  vec3 d6 = st - c6;
  vec3 d7 = st - c7;
  vec3 d8 = st - c8;

  // dotGridGradient: 
  float n1 = dot(g1, d1);
  float n2 = dot(g2, d2);
  float n3 = dot(g3, d3);
  float n4 = dot(g4, d4);
  float n5 = dot(g5, d5);
  float n6 = dot(g6, d6);
  float n7 = dot(g7, d7);
  float n8 = dot(g8, d8);
  
  // interpolation:
  // weights:
  vec3 smooth_st = improved_smooth_step(st - vec3(x0, y0, z0));
  vec3 w = st - vec3(x0, y0, z0);

  float ix1 = smooth_st.x * (n2 - n1) + n1;
  float ix2 = smooth_st.x * (n4 - n3) + n3;
  float ix3 = smooth_st.x * (n6 - n5) + n5;
  float ix4 = smooth_st.x * (n8 - n7) + n7;

  float iy1 = smooth_st.y * (ix2 - ix1) + ix1;
  float iy2 = smooth_st.y * (ix4 - ix3) + ix3;

  float iz = smooth_st.z * (iy2 - iy1) + iy1;

  float improved = 0.5 * iz;
  return improved/sqrt(3);
  /////////////////////////////////////////////////////////////////////////////
}

