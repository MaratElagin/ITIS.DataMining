import main

print('hello from liker')
geckoDriver = main.gecko_driver

#get instagram page
geckoDriver.get("https://www.instagram.com/")

main.login()
main.searchAndOpenProfile()

posts = main.get_posts()
for post in posts:
  geckoDriver.get(post)
  geckoDriver.find_element_by_xpath("//span[contains(@class,'fr66n')]/button").click()
print('All posts have been liked!')