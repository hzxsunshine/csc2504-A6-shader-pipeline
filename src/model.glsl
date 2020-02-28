// Construct the model transformation matrix. The moon should orbit around the
// origin. The other object should stay still.
//
// Inputs:
//   is_moon  whether we're considering the moon
//   time  seconds on animation clock
// Returns affine model transformation as 4x4 matrix
//
// expects: identity, rotate_about_y, translate, PI
mat4 model(bool is_moon, float time)
{
  /////////////////////////////////////////////////////////////////////////////
  // Replace with your code 
  // return identity();
  /////////////////////////////////////////////////////////////////////////////
  float period = 4.0;
  float theta = 2 * M_PI * time / period; // note here, the period we assume is 4 second.

  if (is_moon) {
    mat4 rotation = rotate_about_y(theta);
    return rotation;
  }
  else{
    return identity();
  }
}
