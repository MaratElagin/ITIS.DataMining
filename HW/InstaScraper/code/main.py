import os
import time
from turtle import pd
import secret
from selenium.webdriver.common.keys import Keys
from selenium import webdriver
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC
from selenium.webdriver.common.by import By
import pandas as pd


#path for geckoDriver file
gecko_driver_location = os.path.join(os.getcwd(), 'geckodriver.exe')
#creating driver
gecko_driver = webdriver.Firefox(executable_path=gecko_driver_location)

#login
def login():
  username = WebDriverWait(gecko_driver, 10).until(EC.element_to_be_clickable((By.NAME, "username")))
  password = WebDriverWait(gecko_driver, 10).until(EC.element_to_be_clickable((By.NAME, "password")))
  username.clear()
  password.clear()
  username.send_keys(secret.username)
  password.send_keys(secret.password)
  gecko_driver.find_element_by_css_selector("button[type='submit']").click()

  #skip pop-ups about save password and notifications
  WebDriverWait(gecko_driver, 10).until(EC.element_to_be_clickable((By.XPATH, "//button[contains(text(), 'Не сейчас')]"))).click()
  WebDriverWait(gecko_driver, 5).until(EC.element_to_be_clickable((By.XPATH, "//button[contains(text(), 'Не сейчас')]"))).click()

#searchbox - redirect to account 
def searchAndOpenProfile():
  searchbox = WebDriverWait(gecko_driver, 10).until(EC.element_to_be_clickable((By.  CSS_SELECTOR, "input[placeholder='Поиск']")))
  searchbox.clear()
  searchbox.send_keys(secret.account)
  time.sleep(1)
  searchbox.send_keys(Keys.ENTER)
  time.sleep(1)
  searchbox.send_keys(Keys.ENTER)

#get posts.
def get_posts():
  urls = set()
  posts = set()
  new_links = set()
  time.sleep(5)
  amount_of_posts = gecko_driver.find_element_by_class_name('g47SY').text

  while len(posts) != (int)(amount_of_posts):
    time.sleep(2)
    links = gecko_driver.find_elements_by_tag_name("a")
    new_links = list(set(links) - set(new_links))
    for link in new_links:
      urls.add(link.get_attribute('href'));
    for url in urls:
      if '/p/' in url:
        posts.add(url)
    gecko_driver.execute_script("window.scrollTo(0, document.body.scrollHeight)")
    gecko_driver.execute_script("window.scrollTo(0, document.body.scrollHeight)")
  return posts

#write mentions_dict data to excel file_name
def dictToExcel(mentions_dict, file_name):
  df = pd.DataFrame(data=mentions_dict, index=[0])
  df = (df.T)
  print (df)
  df.to_excel(file_name)


