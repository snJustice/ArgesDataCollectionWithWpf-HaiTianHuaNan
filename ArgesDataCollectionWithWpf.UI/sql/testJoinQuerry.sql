select "Line1".data0,"Line1".data1,"Line1".data2,"Line1".data3,"Line1".data4,
	
	"Line2".data0,"Line2".data1,"Line2".data2
	from public."Line1" left join public."Line2" on  "Line1".data0 = "Line2".data0 
	where "Line2".data2<>''
	limit 10000