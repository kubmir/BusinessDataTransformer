import os
import matplotlib.pyplot as plt

from scipy import stats

from dataLoader import filterInvalidData, loadDataFramesFromFile

def generateProbabiltyPlot(df, currentAnalyzedCol, axs, row, col):
    filteredAnalyzedData = filterInvalidData(df, currentAnalyzedCol)
    
    stats.probplot(filteredAnalyzedData, plot=axs[row, col])

def main():
    df = loadDataFramesFromFile()

    fig, axs = plt.subplots(2, 3, sharey=True, tight_layout=True)

    generateProbabiltyPlot(df, "ROA2012", axs, 0, 0)
    generateProbabiltyPlot(df, "ROA2013", axs, 0, 1)
    generateProbabiltyPlot(df, "ROA2014", axs, 0, 2)

    generateProbabiltyPlot(df, "ROE2012", axs, 1, 0)
    generateProbabiltyPlot(df, "ROE2013", axs, 1, 1)
    generateProbabiltyPlot(df, "ROE2014", axs, 1, 2)

    plt.savefig(os.path.expanduser("~/Desktop/Diplomovka_ESF/outputs/probability_normality.png"))
    plt.show()

if __name__ == "__main__":
    main()