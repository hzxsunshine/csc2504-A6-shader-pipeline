// Add (hard code) an orbiting (point or directional) light to the scene. Light
// the scene using the Blinn-Phong Lighting Model.
//
// Uniforms:
uniform mat4 view;
uniform mat4 proj;
uniform float animation_seconds;
uniform bool is_moon;
// Inputs:
in vec3 sphere_fs_in;
in vec3 normal_fs_in;
in vec4 pos_fs_in; 
in vec4 view_pos_fs_in; 
// Outputs:
out vec3 color;
// expects: PI, blinn_phong
void main()
{
  /////////////////////////////////////////////////////////////////////////////
  // Replace with your code
  vec3 ka;
  vec3 ks;
  vec3 kd;
  float p = 1000;


  if (is_moon){
    ka = vec3(0.01,0.01,0.01), kd = vec3(0.5,0.5,0.5), ks = vec3(0.8,0.8,0.8);
  }
  else{
    ka = vec3(0.01,0.01,0.01), kd = vec3(0.1,0.1,0.9), ks = vec3(0.8,0.8,0.8);
  }


  float period = 4.0;
  float theta = 2 * M_PI * animation_seconds / period;

  vec4 light_pos = vec4(cos(theta)*2, 1.4, sin(theta)*2, 1);
  vec4 light = view * light_pos;

  vec3 d_light = light.xyz;
  vec3 eye = vec3(0, 0 , 0); // eye point is at (0, 0, 0)?
  vec3 pos = view_pos_fs_in.xyz;

  vec3 n = normalize(normal_fs_in);
  vec3 v = normalize(eye - pos);
  vec3 l = normalize(d_light - pos);

  color = blinn_phong(ka, kd, ks, p, n, v, l);
  /////////////////////////////////////////////////////////////////////////////
}