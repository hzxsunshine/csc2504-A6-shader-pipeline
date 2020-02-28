// Set the pixel color using Blinn-Phong shading (e.g., with constant blue and
// gray material color) with a bumpy texture.
// 
// Uniforms:
uniform mat4 view;
uniform mat4 proj;
uniform float animation_seconds;
uniform bool is_moon;
// Inputs:
//                     linearly interpolated from tessellation evaluation shader
//                     output
in vec3 sphere_fs_in;
in vec3 normal_fs_in;
in vec4 pos_fs_in; 
in vec4 view_pos_fs_in; 
// Outputs:
//               rgb color of this pixel
out vec3 color;
// expects: model, blinn_phong, bump_height, bump_position,
// improved_perlin_noise, tangent
void main()
{
  /////////////////////////////////////////////////////////////////////////////
  // Replace with your code 
  vec3 ka;
  vec3 ks;
  vec3 kd;
  float p = 1000;

  mat4 model = model(is_moon, animation_seconds);


  if (is_moon){
    ka = vec3(0.05,0.05,0.05), kd = vec3(0.5,0.5,0.5), ks = vec3(0.8,0.8,0.8);
  }
  else{
    ka = vec3(0.05,0.05,0.05), kd = vec3(0.1,0.1,0.9), ks = vec3(0.8,0.8,0.8);
  }

  vec3 T, B;
  tangent(sphere_fs_in, T, B);

  float epsilon = 0.0001;
  vec3 bump = bump_position(is_moon, sphere_fs_in);

  vec3 dp_dT = (bump_position(is_moon, sphere_fs_in + T * epsilon) - bump) / epsilon;
  vec3 dp_dB = (bump_position(is_moon, sphere_fs_in + B * epsilon) - bump) / epsilon;
  
  vec3 temp = normalize(cross(dp_dT, dp_dB));
  vec3 normal = (transpose(inverse(view)) * transpose(inverse(model)) * vec4(temp,1)).xyz;

  float period = 4.0;
  float theta = 2 * M_PI * animation_seconds / period;

  vec4 light_pos = vec4(cos(theta)*2, 1.4, sin(theta)*2, 1);
  vec4 light = view * light_pos;

  vec3 d_light = light.xyz;
  vec3 eye = vec3(0, 0 , 0); // eye point is at (0, 0, 0)?
  vec3 pos = view_pos_fs_in.xyz;

  vec3 n = normalize(normal);
  vec3 v = normalize(eye - pos);
  vec3 l = normalize(d_light - pos);

  color = blinn_phong(ka, kd, ks, p, n, v, l);
  /////////////////////////////////////////////////////////////////////////////
}
