from System.Collections import *
from System.Collections.Generic import *
#from Hyt.Model import *

import clr
clr.AddReference("System.Xml")
clr.AddReference("Hyt.Model")
from System.Xml import *
from Hyt.Model import *
doc=XmlDocument()
#doc.Load("test.xml")

class MyClass:
    print ("A simple example class")
    i = 12345
    def f(self):
        return 'hello world'
    print (i)
    print(__doc__)
    print (f(i))
    raw_input()
#import time
#time.sleep(10)
#raw_input() 

def reverse(data):
    for index in range(len(data)-1, -1, -1):
            yield data[index]
            
for char in reverse('golf'):
    print (char)
raw_input()


list1=ArrayList()
list1.Add(1)
list1.Add(2)
list1.Add(3)
print (list.Count)
ht=Hashtable()
ht.Add(1,1)
ht.Add('2',2)

print ('aaaa')
print (dir(ht))

promotions=List[SpPromotion]()
promotion = SpPromotion()
promotion.IsAutoChoosed=True
promotion.AvailableGiftNumber=10
promotions.Add(promotion)
promotions.Add(promotion)

print(promotions.Count)
print(promotion.IsAutoChoosed)
print(promotion.AvailableGiftNumber)
print ('-----------------------------------------------------------')
print (dir(promotion))
print ('--------------------Linq---------------------------------------')
import clr
import System
clr.AddReference("System.Core")
clr.ImportExtensions(System.Linq)
# will print 3 and 4 :)
[2, 3, 4,"a","b"].Where(lambda x : x != 2 and x != 3).ToList().ForEach(System.Console.WriteLine)
print ('--------------------Object Null Check---------------------------------------')
isnull = promotions.FirstOrDefault(lambda x : x.IsAutoChoosed == True)
print (isnull==None)

raw_input()
