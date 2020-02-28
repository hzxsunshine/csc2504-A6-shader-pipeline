// Determine the perspective projection (do not conduct division) in homogenous
// coordinates. If is_moon is true, then shrink the model by 70%, shift away
// from the origin by 2 units and rotate around the origin at a frequency of 1
// revolution per 4 seconds.
//
// Uniforms:
//                  4x4 view transformation matrix: transforms "world
//                  coordinates" into camera coordinates.
uniform mat4 view;
//                  4x4 perspective projection matrix: transforms
uniform mat4 proj;
//                                number of seconds animation has been running
uniform float animation_seconds;
//                     whether we're rendering the moon or the other object
uniform bool is_moon;
// Inputs:
//                  3D position of mesh vertex
in vec3 pos_vs_in; 
// Ouputs:
//                   transformed and projected position in homogeneous
//                   coordinates
out vec4 pos_cs_in; 
// expects: PI, model
void main()
{
  vec4 init_vs_in = vec4(pos_vs_in, 1.0);
  mat4 moon_model;

  float theta = M_PI * animation_seconds / 2;
  vec4 shift = vec4(sin(theta)*2, 0, cos(theta)*2, 0);
  vec4 temp;

  if (is_moon){
    moon_model = model(is_moon, animation_seconds) * uniform_scale(0.3);
    temp = moon_model * init_vs_in + shift;
    //vec3 v = vec3(0, 0, 2.0);
    //moon_model = model(is_moon, animation_seconds) * transpose(translate(v)) * uniform_scale(0.3);
  }
  else {
    moon_model = model(is_moon, animation_seconds);
    temp = moon_model * init_vs_in;
  }

  pos_cs_in = proj * view * temp;
}
