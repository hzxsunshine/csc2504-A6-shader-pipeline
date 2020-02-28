// Generate a procedural planet and orbiting moon. Use layers of (improved)
// Perlin noise to generate planetary features such as vegetation, gaseous
// clouds, mountains, valleys, ice caps, rivers, oceans. Don't forget about the
// moon. Use `animation_seconds` in your noise input to create (periodic)
// temporal effects.
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

  float period = 12.0;
  float theta = 2 * M_PI * animation_seconds / period;

  mat4 model = model(is_moon, animation_seconds);

  vec3 T, B;
  tangent(sphere_fs_in, T, B);

  float epsilon = 0.0001;

  vec3 bump = bump_position(is_moon, sphere_fs_in);

  vec3 dp_dT = (bump_position(is_moon, sphere_fs_in + T * epsilon) - bump) / epsilon;
  vec3 dp_dB = (bump_position(is_moon, sphere_fs_in + B * epsilon) - bump) / epsilon;

  vec3 temp = normalize(cross(dp_dT, dp_dB));
  vec3 normal = (transpose(inverse(view)) * transpose(inverse(model)) * vec4(temp,1)).xyz;


  vec4 light_pos;
  float offset = - M_PI / 2 + 2 * M_PI * 7 / period;
  if (animation_seconds >= 7){
    light_pos = vec4(cos(theta - offset)*2, 1.4, (sin(theta - offset))*2, 1);
  }
  else{
    light_pos = vec4(0, 1.4, 2, 1);
  }

  
  //vec4 light_pos = vec4(0, 1, 2, 1); //stable light: debug using
  vec4 light = view * light_pos;

  vec3 d_light = light.xyz;
  vec3 eye = vec3(0, 0 , 0); // eye point is at (0, 0, 0)?
  vec3 pos = view_pos_fs_in.xyz;

  vec3 n = normalize(normal);
  vec3 v = normalize(eye - pos);
  vec3 l = normalize(d_light - pos);

  // time -> height(color)
  // height <= 0 : blue
  // height > 0 : yellow / green

  // initally, earth has 3 strips: green / yellow / green

  // cloud ?
  
  // cloud and mountain texture:
  // add cloud
  vec3 cloud = vec3(5, 5, 5);
  vec3 f = vec3(1, 7, 1);
  float c_cloud = improved_perlin_noise(f * (rotate_about_y(animation_seconds / 3) * vec4(sphere_fs_in, 1)).xyz);

  float time_cloud = (animation_seconds - 4) / 3;
  if (time_cloud < 0){
    c_cloud = 0;
  }
  else if ((time_cloud < 1 && (time_cloud > 0))){
    c_cloud = time_cloud * c_cloud;
  }

  // sea level:
  float sea_level = min(((animation_seconds - 62) * 0.0005), 0.015);
  
  // add mountain texture
  float mountain = improved_perlin_noise(10 * sphere_fs_in);
  float phi = M_PI * sphere_fs_in.y;

  if (is_moon) {
    ka = vec3(0.05,0.05,0.05), kd = vec3(.5,.5,.5), ks = vec3(0.8,0.8,0.8);
  }
  else {
    float height = bump_height(is_moon, sphere_fs_in);
  	if (height < sea_level) {
  		ka = vec3(0.05,0.05,0.05);
      kd = vec3(0.2,0.3,0.6);
      ks = vec3(0.8,0.8,0.8);
  	}
    else {
      kd = vec3(0.88,0.858,0) * cos(phi) + 
              vec3(0.45, 0.75, 0) * (1- cos(phi)) ;
      ka = vec3(0.01,0.01,0.01);
      kd = kd + mountain;
      ks = vec3(0.01,0.01,0.01);
    }
    kd = kd * (1 - c_cloud) + c_cloud * cloud;
  }

  color = blinn_phong(ka, kd, ks, p, n, v, l);
  /////////////////////////////////////////////////////////////////////////////
}
