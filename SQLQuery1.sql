create table tasks (
id int IDENTITY(1,1) PRIMARY KEY,
name varchar(100),
status varchar(15)
);

insert into tasks values ('Buy groceries','Active');
insert into tasks values('Do laundry','Complete');
insert into tasks values('Wish Dad - HBD','Active');

select * from tasks;


select * from tasks where status='Active';

select
  count(case when status='Complete' then 1 end) as complete_cnt,
  count(case when status='Active' then 1 end) as active_cnt,
  count(*) as total_cnt
from tasks;

