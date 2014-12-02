%% The Diferential Robot
% This is a simulation program: 
%   the input are the sensors readings
%   the output is the robot trajectory

%% Input data

%Related to the robot geometri:
%   L the distance betwen the wheels; 
%   r the weehls radius
L=80; % distance [cm]
r=20; %radius [cm]
%Related to the initial position and orientation of the robot
%   the position:
%   the orientation: x_0, y_0, tet_0
x_0=10;% x position
y_0=20;% y position
tet_0=pi/4;% orientation
%Related to the senzor increments and the reading discretisation
%   d_tet, dt
d_tet=1/1024; % increment
dt=1; %sec
%Related to the slippages alf=[0(total slippage)...1(no slippage)]
alfa_r=1;
alfa_l=1;

%% Simulation
%choose the var
var=1;
if var==1 % no diference betwen readings
    senzor_r=ones(1,10000);
    senzor_l=ones(1,10000);
elseif var==2 % oposite readings
    senzor_r=-ones(1,10000);
    senzor_l=ones(1,10000);
elseif var==3 % no reading from the r senzor
      senzor_r=zeros(1,30000);
      senzor_l=ones(1,30000); 
else % extra 
    senzor_r=1.1*ones(1,10000);
    senzor_l=ones(1,10000);
end
figure
hold
stem(senzor_r(1,1:50),'r')
stem(senzor_l(1,1:50),'b')
xlabel 'Senzor readings: (left=blue)(right=red)'
grid


n=length(senzor_l);
x_c=x_0;
y_c=y_0;
tet_c=tet_0;
X=[];Y=[];Tet=[];

for i=1:n
    % the angular speed of the r and l wheels
    omg_r=d_tet*senzor_r(1,i)/dt;
    omg_l=d_tet*senzor_l(1,i)/dt;
    % the liniar speed of the r and l wheels
    v_r=omg_r*alfa_r*r;
    v_l=omg_l*alfa_l*r;
    % the liniar and the angular speed of the robot
    v=0.5*(v_r+v_l);
    omeg=(v_r-v_l)/L;
    % the orientation and position of the robot
    tet_c=tet_c+omeg*dt;
    x_c=x_c+v*cos(tet_c)*dt;
    y_c=y_c+v*sin(tet_c)*dt;
    Tet=[Tet,tet_c];
    X=[X,x_c];
    Y=[Y,y_c];
end

%% The trajectory representation
figure
plot(X,Y)
xlabel 'X [cm]'
ylabel 'Y [cm]'
axis 'equal'
grid