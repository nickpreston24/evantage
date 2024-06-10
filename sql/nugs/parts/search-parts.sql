SELECT *
FROM parts
where id = @id
   or cost = @cost
   
   or kind = @kind
   OR kind like '%' + @kind + '%'

   or name = @name
   OR name like '%' + @name + '%'

   or manufacturer = @manufacturer
   OR manufacturer like '%' + @manufacturer + '%'
;
