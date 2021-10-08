from scipy import stats

from dataLoader import filterInvalidData, loadDataFramesFromFile

def executeShapiroTest(filteredAnalyzedData):
    alpha = 0.05
    statistics, pvalue = stats.shapiro(filteredAnalyzedData)

    if pvalue < alpha:  # null hypothesis: x comes from a normal distribution
        print("Shapiro => data are not normally distributed")
    else:
        print("Shapiro => we cannot reject data are normally distributed")

def executeStandardNormalityTest(filteredAnalyzedData):
    alpha = 0.05
    k2, p = stats.normaltest(filteredAnalyzedData)

    if p < alpha:  # null hypothesis: x comes from a normal distribution
        print("X comes from a normal distribution can be rejected")
    else:
        print("X comes from a normal distribution cannot be rejected")


def executeNormalityTest(df, currentAnalyzedCol):
    filteredAnalyzedData = filterInvalidData(df, currentAnalyzedCol)

    print(currentAnalyzedCol)
    executeStandardNormalityTest(filteredAnalyzedData)
    executeShapiroTest(filteredAnalyzedData)

def main():
    df = loadDataFramesFromFile()

    executeNormalityTest(df, "ROA2012")
    executeNormalityTest(df, "ROA2013")
    executeNormalityTest(df, "ROA2014")
    executeNormalityTest(df, "ROE2012")
    executeNormalityTest(df, "ROE2013")
    executeNormalityTest(df, "ROE2014")

if __name__ == "__main__":
    main()