import statsmodels.api as sm
import pylab as py

from dataLoader import filterInvalidData, loadDataFramesFromFile

def generateQqPlot(df, name):
    filteredAnalyzedData = filterInvalidData(df, name)

    sm.qqplot(filteredAnalyzedData)
    py.show()

def main():
    df = loadDataFramesFromFile()

    generateQqPlot(df, "ROA2012")
    generateQqPlot(df, "ROA2013")
    generateQqPlot(df, "ROA2014")
    generateQqPlot(df, "ROE2012")
    generateQqPlot(df, "ROE2013")
    generateQqPlot(df, "ROE2014")

if __name__ == "__main__":
    main()