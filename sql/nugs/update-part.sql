update parts
set url      = @url
  , kind     = @kind
  , imageurl = @imageurl
where name = @name
   OR name like '%' + @name + '%';
