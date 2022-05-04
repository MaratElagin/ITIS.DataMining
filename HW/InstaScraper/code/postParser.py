from threading import activeCount
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC
from selenium.webdriver.common.by import By
import main

geckoDriver = main.gecko_driver

#get instagram page
geckoDriver.get("https://www.instagram.com/")

main.login()
main.searchAndOpenProfile()


#key - href of account, value - amount of mentions
mentions_dict = dict()

#key - name of account, value - amount of mentions
accounts_dict=dict()

counter = 0
posts = main.get_posts()
for post in posts:
  print(counter)
  counter+=1
  mentions = []
  main.gecko_driver.get(post)
  links = main.gecko_driver.find_elements_by_xpath("//div[@class='C7I1f X7jCj']//span[contains(@class, '_7UhW9')]/a")
  for link in links:
    mention = link.get_attribute('href')
    if not 'tags' in mention:
      mentions.append(mention)
  for mention in mentions:
      if mention not in mentions_dict:
       mentions_dict[mention] = 1   
      else: mentions_dict[mention]+=1

for mention_href in mentions_dict:
 main.gecko_driver.get(mention_href)
 try:
   name_of_account = WebDriverWait(main.gecko_driver, 10).until(EC.element_to_be_clickable((By.XPATH, "//div[@class='QGPIr']//span[contains(@class, '_7UhW9')]"))).text
 except:
   name_of_account = ''
 accounts_dict[name_of_account] = mentions_dict[mention_href]

main.dictToExcel(accounts_dict, 'data.xlsx')
print('all posts have been parsed!')