%% The Diferential Robot
% This is a simulation program: 
%   the input are the wheels angular speed
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
tet_0=0;% orientation
%Related to the reading discretisation
%   dt
dt=1; %sec
%Related to the slippages alf=[0(total slippage)...1(no slippage)]
alf_r=1;
alf_l=1;

%% Simulation
%choose the var
var=2;
if var==1 % no diference betwen readings
    omg_r=ones(1,10000);
    omg_l=ones(1,10000);
else 
    omg_r=sin(0:0.1:2*pi)
    omg_l=cos(0:0.1:2*pi)
end
figure
hold
plot(omg_r,'r')
plot(omg_l,'b')
xlabel 'Wheels angular speed: (left=blue)(right=red)'
grid


n=length(omg_r)
x_c=x_0;
y_c=y_0;
tet_c=tet_0;
X=[];Y=[];Tet=[]

for i=1:n
    % the liniar speed of the r and l wheels
    v_r=omg_r(1,i)*alf_r*r;
    v_l=omg_l(1,i)*alf_l*r;
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
% The orientation representation
figure
for i=1:n
    plot(cos(0:0.1:2*pi),sin(0:0.1:2*pi),'.')
    hold
    plot([0,cos(Tet(1,i))],[0,sin(Tet(1,i))], 'lineWidth',2)
    grid
    axis 'equal'
    pause (0.1)
    hold
end