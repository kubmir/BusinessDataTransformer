import os
import matplotlib.pyplot as plt

from scipy import stats

from dataLoader import filterInvalidData, loadDataFramesFromFile

def generateBoxPlot(df, currentAnalyzedCol, axs, row, col):
    filteredAnalyzedData = filterInvalidData(df, currentAnalyzedCol)
    
    axs[row, col].boxplot(filteredAnalyzedData)
    axs[row, col].title.set_text(currentAnalyzedCol)

def main():
    df = loadDataFramesFromFile()

    fig, axs = plt.subplots(2, 3, sharey=True, tight_layout=True)

    generateBoxPlot(df, "ROA2012", axs, 0, 0)
    generateBoxPlot(df, "ROA2013", axs, 0, 1)
    generateBoxPlot(df, "ROA2014", axs, 0, 2)

    generateBoxPlot(df, "ROE2012", axs, 1, 0)
    generateBoxPlot(df, "ROE2013", axs, 1, 1)
    generateBoxPlot(df, "ROE2014", axs, 1, 2)

    plt.savefig(os.path.expanduser("~/Desktop/Diplomovka_ESF/outputs/box_plot.png"))
    plt.show()

if __name__ == "__main__":
    main()