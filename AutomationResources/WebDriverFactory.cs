﻿using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;
using System.Reflection;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;

namespace AutomationResources
{
    public class WebDriverFactory
    {
        public IWebDriver Create(BrowserType browserType)
        {
            switch (browserType)
            {
                case BrowserType.Chrome:
                    return GetChromeDriver();
                case BrowserType.FireFox:
                    return GetFireFoxDriver();
                case BrowserType.IE:
                    return GetIEDriver();
                default:
                    throw new ArgumentOutOfRangeException("No such browser exists");
            }
        }

        private IWebDriver GetIEDriver()
        {
            return new InternetExplorerDriver(GetSeleniumBinaryLocation());
        }

        private IWebDriver GetFireFoxDriver()
        {
            return new FirefoxDriver(GetSeleniumBinaryLocation());
        }

        private IWebDriver GetChromeDriver()
        {
            return new ChromeDriver(GetSeleniumBinaryLocation());
        }

        public IWebDriver CreateSauceDriver(string browser, string version, string os, string deviceName, string deviceOrientation)
        {
            var capabilities =  new DesiredCapabilities();
            capabilities.SetCapability(CapabilityType.BrowserName, browser);
            capabilities.SetCapability(CapabilityType.Version, version);
            capabilities.SetCapability(CapabilityType.Platform, os);
            capabilities.SetCapability("deviceName", deviceName);
            capabilities.SetCapability("deviceOrientation", deviceOrientation);
            capabilities.SetCapability("username", 
                Environment.GetEnvironmentVariable("SAUCE_USERNAME", EnvironmentVariableTarget.User));
            capabilities.SetCapability("accessKey", 
                Environment.GetEnvironmentVariable("SAUCE_ACCESS_KEY", EnvironmentVariableTarget.User));
            return new RemoteWebDriver(new Uri("http://ondemand.saucelabs.com:80/wd/hub"),
                capabilities, TimeSpan.FromSeconds(600));
        }

        public IWebDriver CreateSauceDriver(SauceConfiguration sauceConfig)
        {
            var driver = CreateSauceDriver(sauceConfig.Browser, sauceConfig.Version, sauceConfig.OS,
                sauceConfig.DeviceName, sauceConfig.DeviceOrientation);
            ((IJavaScriptExecutor)driver).ExecuteScript($"sauce:job-name={sauceConfig.TestCaseName}");
            return driver;
        }

        public IWebDriver CreateRemoteDriver()
        {
            var caps = DesiredCapabilities.Chrome();
            caps.SetCapability(CapabilityType.Platform, "Windows 10");

            var options = new ChromeOptions();
            options.BinaryLocation = GetSeleniumBinaryLocation();
            //---- >>>> Don't do this - Setting the browser name is redundant
            //options.AddAdditionalCapability(CapabilityType.BrowserName, "chrome", true);
            //options.AddAdditionalCapability(CapabilityType.Version, "48.0", true);
            options.AddAdditionalCapability(CapabilityType.Platform, "Windows 10", true);

            //3. IMPORTANT - Notice the options.ToCapabilities() call!!
            return new RemoteWebDriver(new Uri("http://localhost:4444/wd/hub"), 
                caps);
        }

        public IWebDriver CreateSauceDriver()
        {
            //DesiredCapabilities caps = new DesiredCapabilities();
            //caps.SetCapability("browserName", "Chrome");
            //caps.SetCapability("platform", "Windows 8.1");
            //caps.SetCapability("version", "62.0");
            //caps.SetCapability("screenResolution", "1024x768");

            var capabilities =  DesiredCapabilities.Chrome();
            //capabilities.SetCapability(CapabilityType.BrowserName, "chrome");
            capabilities.SetCapability(CapabilityType.Version, "48.0");
            capabilities.SetCapability(CapabilityType.Platform, "Linux");
            capabilities.SetCapability("username", 
                Environment.GetEnvironmentVariable("SAUCE_USERNAME", EnvironmentVariableTarget.User));
            capabilities.SetCapability("accessKey", 
                Environment.GetEnvironmentVariable("SAUCE_ACCESS_KEY", EnvironmentVariableTarget.User));
            return new RemoteWebDriver(new Uri("http://ondemand.saucelabs.com:80/wd/hub"),
                capabilities, TimeSpan.FromSeconds(600));
        }

        private string GetSeleniumBinaryLocation()
        {
            var outPutDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return Path.GetFullPath(Path.Combine(outPutDirectory ?? throw new InvalidOperationException(), @"..\..\..\AutomationResources\bin\Debug"));
        }
    }
}
